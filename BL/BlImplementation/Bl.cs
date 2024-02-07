namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Method to automatically create project schedule
    /// </summary>
    /// <param name="projectStartDate"> project start date </param>
    /// <exception cref="BO.BlInvalidValueException"> if can not plan project schedule because not all tasks have required effort time assigned </exception>
    public void CreateProjectSchedule(DateTime projectStartDate)
    {
        //reading tasks and saving in sorted list
        List<BO.TaskInList> doTasks = Task.ReadAll().OrderBy(item => item.Id).ToList();
        //making sure all tasks have required effort time assigned
        if (doTasks.Any(task => Task.Read(task.Id).RequiredEffortTime == null))
            throw new BO.BlInvalidValueException("Can not plan project schedule if not all tasks have required effort time assigned");
        
        //for each task, finding the earliest possible date and assigning to task
        DateTime? date;
        doTasks.ForEach(task =>
        {
            date = GetEarliestDate(Task.Read(task.Id), projectStartDate);
            if (date == null) throw new BO.BlInvalidValueException("Forcast date is null");
            Task.AssignScheduledDateToTask(task.Id, (DateTime)date);
        });
        
        //updating project start date in config
        Dal.Config.ProjectStartDate = projectStartDate;
        
    }

    /// <summary>
    /// Help method to find task's earliest possible start date
    /// </summary>
    /// <param name="task"> task to find date for </param>
    /// <param name="projectStartDate"> project start date </param>
    /// <returns> earliest possible start date </returns>
    /// <exception cref="BO.BlAssignmentImpossibleException"></exception>
    DateTime? GetEarliestDate(BO.Task task, DateTime projectStartDate)
    {
        //if there are no previos tasks the task scheduled start date is the project start date
        if (task.Dependencies == null || task.Dependencies.Count == 0)
            return projectStartDate;

        //reading previos tasks 
        var previosTasks = from dependency in task.Dependencies
                           select Task.Read(dependency.Id);

        //checking if any previos tasks dont have a scheduled start date yet
        if (previosTasks.Any(task => task.ScheduledDate == null))
            throw new BO.BlAssignmentImpossibleException("Can not assign scheduled start date to task if previos tasks dont have a scheduled start date yet");

        //finding maximal planned finish date of previos tasks
        return previosTasks.Max(task => task.ForecastDate);

    }

    /// <summary>
    /// method to reset all data
    /// </summary>
    public void Reset()  
    {
        DalApi.IDal _dal = DalApi.Factory.Get;
        _dal.Reset();
        RemoveStartDate("data-config", "ProjectStartDate");
    }

    public void Initialize()
    {
        Reset();
        DalTest.Initialization.Do();
    }

    /// <summary>
    /// method to get project status according to project start date
    /// </summary>
    /// <returns> project status </returns>
    public ProjectStatus GetProjectStatus()
    {
        return Dal.Config.ProjectStartDate != null ? ProjectStatus.InExecution : ProjectStatus.InPlanning;
    }

    #region remove date from xml file
    //help method
    static XElement LoadListFromXMLElement(string entity)
    {
        const string s_xml_dir = @"..\xml\";
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    /// <summary>
    /// help method to remove project start date from config
    /// </summary>
    /// <param name="data_config_xml"> file name </param>
    /// <param name="elemName"> tag name </param>
    static void RemoveStartDate(string data_config_xml, string elemName)
    {
        const string s_xml_dir = @"..\xml\";
        string filePath = $"{s_xml_dir + data_config_xml}.xml";
        XElement root = LoadListFromXMLElement(data_config_xml);
        if (root.Elements(elemName).Any())
            (root.Element(elemName)!).Remove();
        root.Save(filePath);
    }
    #endregion
}

