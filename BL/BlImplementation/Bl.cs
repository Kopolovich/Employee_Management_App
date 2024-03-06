namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation(this);

    public IEngineer Engineer => new EngineerImplementation(this);

    public IUser User => new UserImplementation(this);

    /// <summary>
    /// Method to automatically create project schedule, returns scheduled finish date for project
    /// </summary>
    /// <param name="projectStartDate"> project start date </param>
    /// <exception cref="BO.BlInvalidValueException"> if can not plan project schedule because not all tasks have required effort time assigned </exception>
    public DateTime CreateProjectSchedule(DateTime projectStartDate)
    {
        if (GetProjectStatus() == ProjectStatus.InExecution)
            throw new BO.BlCreationImpossibleException("Can not create project schedule while project is in Execution stage");
        
        //reading tasks and saving in sorted list
        List<BO.Task> tasks = Task.ReadAllFullTasks().OrderBy(item => item.Id).ToList();
        
        //making sure all tasks have required effort time assigned
        if (tasks.Any(task => task.RequiredEffortTime == null))
            throw new BO.BlInvalidValueException("Can not plan project schedule if not all tasks have required effort time assigned");
        
        //for each task, finding the earliest possible date and assigning to task
        tasks.ForEach(task => RecursiveProjectSchedule(task.Id, projectStartDate));     
        
        //updating project start date in config
        Dal.Config.ProjectStartDate = projectStartDate;

        //finding maximal planned finish date of tasks
        return (DateTime)tasks.Max(task => task.ForecastDate)!;
    }

    void RecursiveProjectSchedule(int id, DateTime projectStartDate) 
    { 
        BO.Task task = Task.Read(id);
        if (task.ScheduledDate != null) return;

        List<BO.TaskInList>? prevTasks = task.Dependencies ?? new List<BO.TaskInList>();

        prevTasks.ForEach(task => RecursiveProjectSchedule(task.Id, projectStartDate));

        DateTime? startDate = GetEarliestDate(Task.Read(task.Id), projectStartDate);

        if (startDate == null) throw new BO.BlInvalidValueException("Forecast date is null");
            Task.AssignScheduledDateToTask(task.Id, (DateTime)startDate);
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
        //if there are no previous tasks the task scheduled start date is the project start date
        if (task.Dependencies == null || task.Dependencies.Count == 0)
            return projectStartDate;

        //reading previous tasks 
        var previousTasks = from dependency in task.Dependencies
                           select Task.Read(dependency.Id);

        //checking if any previous tasks don't have a scheduled start date yet
        if (previousTasks.Any(task => task.ScheduledDate == null))
            throw new BO.BlAssignmentImpossibleException("Can not assign scheduled start date to task if previous tasks don't have a scheduled start date yet");

        //finding maximal planned finish date of previous tasks
        return previousTasks.Max(task => task.ForecastDate);

    }

    /// <summary>
    /// method for gantt chart, to get project dates
    /// </summary>
    /// <returns> list of all dates between project start date and scheduled finish date </returns>
    public List<DateTime> GetProjectDates()
    {
        List<BO.Task> tasks = Task.ReadAllFullTasks().ToList();

        DateTime firstDate = (DateTime)Dal.Config.ProjectStartDate!;

        //finding maximal planned finish date of tasks
        DateTime lastDate = (DateTime)tasks.Max(task => task.ForecastDate)!;

        List<DateTime> projectDates = [];

        projectDates.Add(firstDate);
        while(firstDate != lastDate) 
        {
            firstDate = firstDate.AddDays(1);
            projectDates.Add(firstDate);
        }

        return projectDates;
    }

    /// <summary>
    /// method to reset all data
    /// </summary>
    public void Reset()  
    {
        DalApi.Factory.Get.Reset();
    }

    /// <summary>
    /// method to initialize data
    /// </summary>
    public void Initialize()
    {
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

    public DateTime? ProjectStartDate { get { return Dal.Config.ProjectStartDate; } }

    #region Clock

    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

      public void AddDay()
    {
        Clock = Clock.AddDays(1);
    }

    public void AddWeek()
    {
        Clock = Clock.AddDays(7);
    }

    public void AddMonth()
    {
        Clock = Clock.AddMonths(1);
    }

    public void AddYear()
    {
        Clock = Clock.AddYears(1);
    }

    public void ResetClock()
    {
        Clock = DateTime.Now.Date;
    }

    #endregion
}

