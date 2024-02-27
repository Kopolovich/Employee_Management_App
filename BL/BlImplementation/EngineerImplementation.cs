namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

internal class EngineerImplementation : IEngineer
{
    private readonly Bl _bl;
    private TaskImplementation _iTask;
    internal EngineerImplementation(Bl bl)
    {
        _bl = bl;
        _iTask = new TaskImplementation(bl);
    } 

    private DalApi.IDal _dal = DalApi.Factory.Get;
    
    /// <summary>
    /// adding new engineer to dal
    /// </summary>
    /// <param name="engineer"> engineer logic entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if received engineer is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of engineer's props contain invalid values </exception>
    /// <exception cref="BO.BlAlreadyExistsException"> if engineer with this Id already exists </exception>
    public void Create(BO.Engineer? engineer)
    {
        try
        {
            if (engineer == null)
                throw new BO.BlNullPropertyException("Engineer is null");
            if (engineer.Id <= 0 || engineer.Name == "" || engineer.Cost <= 0 || !CheckEmail(engineer.Email!))
                throw new BO.BlInvalidValueException("Engineer with invalid values");

            //adding engineer using dal Create method
            _dal.Engineer.Create(new DO.Engineer(engineer.Id, (DO.EngineerExperience)engineer.Level, engineer.Email, engineer.Cost, engineer.Name));

        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer!.Id} already exists", ex);
        }
    }

    /// <summary>
    /// reads engineer
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    /// <returns> logic engineer entity </returns>
    /// <exception cref="BO.BlDoesNotExistException"> if engineer with given id does not exist </exception>
    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist");

        //finding all tasks assigned to engineer
        IEnumerable<DO.Task>? dTasks = _dal.Task.ReadAll(item => item.EngineerId == id);

        //finding current task that engineer is working on
        DO.Task? currentTask = (from DO.Task dTask in dTasks
                               where _iTask.Read(dTask.Id).Status == BO.Status.OnTrack
                               select dTask).FirstOrDefault();                               

        //defining task in engineer help entity to contain current task info
        BO.TaskInEngineer? taskInEngineer = null;
        if (currentTask != null)
            taskInEngineer = new BO.TaskInEngineer() { Id = currentTask.Id, Alias = currentTask.Alias };
        
        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = taskInEngineer
        };
    }
    
    /// <summary>
    /// reads collection of all engineers
    /// </summary>
    /// <param name="filter"> optional filter to filter engineers </param>
    /// <returns> collection of logic engineer entities </returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        if(filter != null)
        {
            return (
             from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
             let boEngineer = Read(doEngineer.Id) //using Read method to create logic entity
             where filter(boEngineer)
             select boEngineer
             );
        }
        return (
            from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
            let boEngineer = Read(doEngineer.Id) //using Read method to create logic entity
            select boEngineer 
            );
    }

    /// <summary>
    /// updates existing engineer
    /// </summary>
    /// <param name="engineer"> updated logic engineer entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if received engineer is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of updated engineer's props contain invalid values </exception>
    /// <exception cref="BO.BlDoesNotExistException"> if engineer does not exist in dal </exception>
    /// <exception cref="BO.BlAlreadyExistsException"> the Create method that is called in dal Update method might throw an exception </exception>
    public void Update(BO.Engineer? engineer)
    {
        try
        {
            if (engineer == null)
                throw new BO.BlNullPropertyException("Engineer is null");
            if (engineer.Id <= 0 || engineer.Name == "" || engineer.Cost <= 0 || !CheckEmail(engineer.Email!))
                throw new BO.BlInvalidValueException("Engineer with invalid values");

            //making sure the updated level is not lower than current level
            DO.Engineer? doEngineer = _dal.Engineer.Read(engineer.Id);
            if (doEngineer == null)
                throw new BO.BlDoesNotExistException($"Engineer with ID={engineer!.Id} does Not exist");
            if (doEngineer.Level > (DO.EngineerExperience)engineer.Level)
                throw new BO.BlInvalidValueException("It is not possible to lower the experience level of an engineer");

            //if (_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning && engineer.Task != null)
            //    throw new BO.BlUpdatingImpossibleException("Can not assign task to engineer while project is still in planning");
            
            //if possible, assign task to engineer
            //if (_bl.GetProjectStatus() == BO.ProjectStatus.InExecution && engineer.Task != null)
            //    AssignTaskToEngineer(engineer, engineer.Task);

            _dal.Engineer.Update(new DO.Engineer(engineer.Id, (DO.EngineerExperience)engineer.Level, engineer.Email, engineer.Cost, engineer.Name));
                      
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer!.Id} already exists", ex);
        }
        catch(DO.DalDoesNotExistException ex) 
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer!.Id} does Not exist", ex);
        }
    }

    /// <summary>
    /// deletes engineer
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    /// <exception cref="BO.BlDeletionImpossibleException"> if engineer is currently working on a task </exception>
    /// <exception cref="BO.BlDoesNotExistException"> if requested engineer does not exist </exception>
    public void Delete(int id)
    {
        //if engineer does not exist in dal the Read method throws an exception
        BO.Engineer engineer = Read(id);

        //checking if there is a task that engineer is currently working on
        if(engineer.Task != null)
        {
            BO.Task boTask = _iTask.Read(engineer.Task.Id);
            if (boTask.Status == BO.Status.OnTrack)
                throw new BO.BlDeletionImpossibleException("Can not delete engineer that is currently working on a task");
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist", ex);
        }
        
    }

    /// <summary>
    /// private help method for assigning task to engineer
    /// </summary>
    /// <param name="engineerId"> id of engineer </param>
    /// <param name="task"> task to be assigned to </param>
    /// <exception cref="BO.BlDoesNotExistException"> if requested task does not exist </exception>
    /// <exception cref="BO.BlAssignmentImpossibleException"> if engineer can not be assigned to task for various reasons </exception>
    public void AssignTaskToEngineer(int engineerId, BO.TaskInEngineer task)
    {
        //checking if project is still in planning
        if (_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning)
            throw new BO.BlUpdatingImpossibleException("Can not assign task to engineer while project is still in planning");

        //checking if engineer is still working on different task
        BO.TaskInEngineer? oldTask = Read(engineerId).Task;
        if (oldTask != null && _iTask.Read(oldTask.Id).Status != BO.Status.Done)
            throw new BO.BlUpdatingImpossibleException("Can not assign a new task to engineer before he finishes working on his current task");

        //checking if task exists
        DO.Task? dTask = _dal.Task.Read(task.Id);
        if (dTask == null) throw new BO.BlDoesNotExistException($"Task with id={task.Id} does not exist");

        //checking if a different engineer is already assigned to task
        if (dTask.EngineerId != null && dTask.EngineerId != engineerId)
            throw new BO.BlAssignmentImpossibleException("A different engineer is already assigned to task");

        //checking if previous tasks are not done yet
        var dependencies = from item in _iTask.Read(task.Id).Dependencies
                           where _iTask.Read(item.Id).Status != BO.Status.Done
                           select item;
        if (dependencies.Any()) throw new BO.BlAssignmentImpossibleException("Previous tasks are not done yet");

        //checking if the complexity of the task is higher than engineer's level
        if (dTask.Complexity > (DO.EngineerExperience)Read(engineerId).Level)
            throw new BO.BlAssignmentImpossibleException("The complexity of the task is higher than engineer's level");

        //assigning task to engineer using dal update method
        _dal.Task.Update(new DO.Task()
        {
            Id = dTask.Id,
            Complexity = dTask.Complexity,
            Alias = dTask.Alias,
            Description = dTask.Description,
            CreatedAtDate = dTask.CreatedAtDate,
            RequiredEffortTime = dTask.RequiredEffortTime,
            StartDate = _bl.Clock,
            ScheduledDate = dTask.ScheduledDate,
            CompleteDate = dTask.CompleteDate,
            Deliverables = dTask.Deliverables,
            Remarks = dTask.Remarks,
            EngineerId = engineerId
        });
    }

    #region Help methods
    /// <summary>
    /// private help method to check if email address is valid
    /// </summary>
    /// <param name="email"> email address </param>
    /// <returns> true if email is valid, else false </returns>
    bool CheckEmail(string email)
    {
        if (email == null)
            return false;
        // Basic regular expression for email validation
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        // Create a Regex object
        Regex regex = new Regex(pattern);
        // Use the IsMatch method to validate the email
        return regex.IsMatch(email);
    }

    #endregion

}
