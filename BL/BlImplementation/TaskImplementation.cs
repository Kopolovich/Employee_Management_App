namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// adding new task to dal
    /// </summary>
    /// <param name="task"> logic task entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if recieved task is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of task's props contain invalid values </exception>
    public void Create(BO.Task? task)
    {
        if (task == null) throw new BO.BlNullPropertyException("Task is null");

        if (task.Id <= 0 || task.Alias == "") throw new BO.BlInvalidValueException("Task with invalid values");
       
        _dal.Task.Create(
            new DO.Task()
            {
                   Id = task.Id,
                   Complexity = (DO.EngineerExperience)task.Complexity,
                   Alias = task.Alias,
                   Description = task.Description,
                   CreatedAtDate = task.CreatedAtDate,
                   RequiredEffortTime = task.RequiredEffortTime,
                   IsMilestone = task.Milestone == null ? false : true,
                   StartDate = task.StartDate,
                   ScheduledDate = task.ScheduledDate,
                   DeadlineDate = task.DeadlineDate,
                   CompleteDate = task.CompleteDate,
                   Deliverables = task.Deliverables,
                   Remarks = task.Remarks,
                   EngineerId = task.Engineer == null ? null : task.Engineer.Id
            });

        // Creating dependencies in dal for all tasks that new task depends on
        if(task.Dependencies != null)
        {
             task.Dependencies.ForEach(depTask => _dal.Dependency.Create(new DO.Dependency() 
             { DependentTask = task.Id, DependsOnTask = depTask.Id }));
        }
    }

    /// <summary>
    /// reads task
    /// </summary>
    /// <param name="id"> id of requested task </param>
    /// <returns> logic task entity </returns>
    /// <exception cref="BO.BlDoesNotExistException"> if task with given id does not exist </exception>
    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if(doTask == null) throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");

        //creating list of tasks that requested task depends on 
        IEnumerable <DO.Dependency?> dependencies = _dal.Dependency.ReadAll(item => item.DependentTask == id);
        List<BO.TaskInList> depTasks = (from dependency in dependencies
                                        let depTask = _dal.Task.Read(dependency.DependsOnTask)
                                        select new BO.TaskInList()
                                        {
                                            Id = depTask.Id,
                                            Description = depTask.Description,
                                            Alias = depTask.Alias,
                                            Status = GetStatus(depTask)
                                        }).ToList();
        
        //finding name of engineer
        int engId = doTask.EngineerId ?? 0;
        DO.Engineer? engineer = null;
        BO.EngineerInTask? engineerInTask = null;
        if (engId != 0)
        {
            engineer = _dal.Engineer.Read(engId);
            engineerInTask = new BO.EngineerInTask() { Id = engineer.Id, Name = engineer.Name };
        }
            
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = GetStatus(doTask),
            Dependencies = depTasks,
            Milestone = null,
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = (doTask.ScheduledDate > doTask.StartDate ? doTask.ScheduledDate : doTask.StartDate) + doTask.RequiredEffortTime,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = engineerInTask,
            Complexity = (BO.EngineerExperience)doTask.Complexity
        };

    }

    /// <summary>
    /// reads collection of all tasks
    /// </summary>
    /// <param name="filter"> optional filter to filter tasks </param>
    /// <returns> collection of logic task entities </returns>
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (
                from DO.Task doTask in _dal.Task.ReadAll(filter)
                let boTask = Read(doTask.Id) //using Read method to create logic entity
                select boTask
                );  
    }

    /// <summary>
    /// Updates existing task
    /// </summary>
    /// <param name="task"> updated logic task entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if recieved task is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of updated task's props contain invalid values </exception>
    /// <exception cref="BO.BlDoesNotExistException"> if task does not exist in dal </exception>
    public void Update(BO.Task? task)
    {
        try
        {
            if (task == null)
                throw new BO.BlNullPropertyException("Task is null");
            if (_dal.Task.Read(task.Id) == null) throw new BO.BlDoesNotExistException($"Task with ID={task.Id} does not exist");
            if (task.Id <= 0 || task.Alias == "")
                throw new BO.BlInvalidValueException("Task with invalid values");

            //Checking if updated task contains a new scheduled date and if it is 
            if (task.ScheduledDate != null && task.ScheduledDate != _dal.Task.Read(task.Id)!.ScheduledDate)
                CheckScheduledDate(task.Id, task.ScheduledDate);
            
            _dal.Task.Delete(task.Id); //deleting old task
            Create(task); //creating updated task
        }
 
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={task!.Id} does Not exist", ex);
        }
    }

    /// <summary>
    /// deletes task
    /// </summary>
    /// <param name="id"> id of requested task </param>
    /// <exception cref="BO.BlDoesNotExistException"> if requested task does not exist </exception>
    /// <exception cref="BO.BlDeletionImpossibleException"> when there are tasks that depend on this task </exception>
    public void Delete(int id)
    {
        if(_dal.Task.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        IEnumerable<DO.Dependency?>? dependencies = _dal.Dependency.ReadAll
          (item => item.DependsOnTask == id && Read(item.DependentTask).Status != BO.Status.Done);
        if (dependencies.Any())
            throw new BO.BlDeletionImpossibleException($"There are tasks that depend on Task with ID={id}");

        try
        {
            _dal.Task.Delete(id);
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist", ex);
        }
       

    }

    /// <summary>
    /// Help method to check if scheduled start date is valid
    /// </summary>
    /// <param name="id"> id of task </param>
    /// <param name="startDate"> new/updated scheduled start date </param>
    /// <exception cref="BO.BlInvalidValueException"> if date is invalid </exception>
    void CheckScheduledDate(int id, DateTime? startDate)
    {
        BO.Task task = Read(id);
        IEnumerable<DO.Task?> depenedOnTasks = from item in task.Dependencies 
                               let doTask = _dal.Task.Read(item.Id)
                               where doTask.ScheduledDate == null || doTask.CompleteDate > startDate
                               select doTask;

        if (depenedOnTasks.Any())
            throw new BO.BlInvalidValueException($"Can not assign requested date to task");

    }

    /// <summary>
    /// Help method to calculate task status
    /// </summary>
    /// <param name="doTask"> dal task entity </param>
    /// <returns> status of task </returns>
    BO.Status GetStatus(DO.Task doTask)
    {
        BO.Status currentStatus = BO.Status.Unscheduled;
        switch (doTask.ScheduledDate, doTask.StartDate, doTask.CompleteDate)
        {
            case var (scheduled, started, completed) when scheduled != null && started == null:
                currentStatus = BO.Status.Scheduled;
                break;

            case var (scheduled, started, completed) when started != null && completed == null:
                currentStatus = BO.Status.OnTrack;
                break;

            case var (scheduled, started, completed) when completed != null:
                currentStatus = BO.Status.Done;
                break;
        }
        return currentStatus;
    }

}
