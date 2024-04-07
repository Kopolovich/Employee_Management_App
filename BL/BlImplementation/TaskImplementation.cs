namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;


internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly Bl _bl;
    internal TaskImplementation(Bl bl) => _bl = bl;

    /// <summary>
    /// adding new task to dal
    /// </summary>
    /// <param name="task"> logic task entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if received task is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of task's props contain invalid values </exception>
    public void Create(BO.Task? task)
    {
        if (task == null) throw new BO.BlNullPropertyException("Task is null");

        if (_bl.GetProjectStatus() == BO.ProjectStatus.InExecution)
            throw new BO.BlCreationImpossibleException("Can not add new task after the project start date was declared");

        //not checking id correctness cuz it is automatically created in dal create method (temporarily 0)
        if(task.Alias == "" || task.Alias == null || task.Description == "" || task.Description == null || task.RequiredEffortTime == null || task.RequiredEffortTime.Value.Days <= 0)
            throw new BO.BlInvalidValueException("Task must contain Alias, Description and Required effort time");

        //Creating new task with allowed props to this stage of the project
        _dal.Task.Create(
            new DO.Task()
            {
                   Complexity = (DO.EngineerExperience)task.Complexity,
                   Description = task.Description,
                   Alias = task.Alias,
                   RequiredEffortTime = task.RequiredEffortTime,
                   Deliverables = task.Deliverables,
                   Remarks = task.Remarks,
                   CreatedAtDate = _bl.Clock,
                   IsActive = true
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
                 
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = GetStatus(doTask),
            Dependencies = GetDependencies(id),
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = GetForcast(doTask),
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = GetEngineerInTask(doTask),
            Complexity = (BO.EngineerExperience)doTask.Complexity
        };
    }

    /// <summary>
    /// reads list of partial tasks
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic taskInList entities </returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if(filter != null) 
        {
            return (
            from DO.Task doTask in _dal.Task.ReadAll(task => task.IsActive)
            let boTask = Read(doTask.Id) //using Read method to create logic entity
            where filter(boTask)
            select new BO.TaskInList()
            {
                Id = boTask.Id,
                Description = boTask.Description,
                Alias = boTask.Alias,
                Status = boTask.Status
            }
            
            ).OrderBy(item => item.Id);
        }

        return (
        from DO.Task doTask in _dal.Task.ReadAll(task => task.IsActive)
        let boTask = Read(doTask.Id) //using Read method to create logic entity
        select new BO.TaskInList() 
        { 
            Id = boTask.Id,
            Description= boTask.Description,
            Alias = boTask.Alias,
            Status = boTask.Status
        }
        ).OrderBy(item => item.Id);

    }

    /// <summary>
    /// reads list of full tasks
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic task entities </returns>
    public IEnumerable<BO.Task> ReadAllFullTasks(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return (
            from DO.Task doTask in _dal.Task.ReadAll(task => task.IsActive)
            let boTask = Read(doTask.Id) //using Read method to create logic entity
            where filter(boTask)
            select boTask
            ).OrderBy(item => item.Id);
        }

        return (
        from DO.Task doTask in _dal.Task.ReadAll(task => task.IsActive)
        let boTask = Read(doTask.Id) //using Read method to create logic entity
        select boTask
        ).OrderBy(item => item.Id);
    }


    /// <summary>
    /// reads list of not active tasks
    /// </summary>
    /// <returns> collection of logic taskInList entities </returns>
    public IEnumerable<BO.TaskInList> ReadAllNotActive()
    {
        return (
        from DO.Task doTask in _dal.Task.ReadAll(task => !task.IsActive)
        let boTask = Read(doTask.Id) //using Read method to create logic entity
        select new BO.TaskInList()
        {
            Id = boTask.Id,
            Description = boTask.Description,
            Alias = boTask.Alias,
            Status = boTask.Status
        }
        ).OrderBy(item => item.Id);
    }

    /// <summary>
    /// Updates existing task
    /// </summary>
    /// <param name="task"> updated logic task entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if received task is null </exception>
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
            
            //Checking if trying to update fields that can not be updated while project is still in planning
            if (_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning)
            {
                if (task.Engineer != null)
                    throw new BO.BlUpdatingImpossibleException("Can not assign engineer to task before the project start date was declared");
                if (task.StartDate != null)
                    throw new BO.BlUpdatingImpossibleException("Can not start working on task before the project start date was declared");
                if (task.CompleteDate != null)
                    throw new BO.BlUpdatingImpossibleException("Can not finish working on task before the project start date was declared");
            }

            //checking if trying to update required effort time while project is in execution
            if(_bl.GetProjectStatus() == BO.ProjectStatus.InExecution)
            {
                if (task.RequiredEffortTime != null && task.RequiredEffortTime != Read(task.Id).RequiredEffortTime )
                    throw new BO.BlUpdatingImpossibleException("Can not change required effort time after the project start date was declared");
               
            }

            //updating dependencies
            updateDependencies(task);          

            _dal.Task.Update(new DO.Task()
            {
                Id = task.Id,
                Complexity = (DO.EngineerExperience)task.Complexity,
                CreatedAtDate = task.CreatedAtDate,
                Description = task.Description,
                Alias = task.Alias,
                RequiredEffortTime = task.RequiredEffortTime,
                StartDate = task.StartDate,
                ScheduledDate = task.ScheduledDate,
                CompleteDate = task.CompleteDate,
                Deliverables = task.Deliverables,
                Remarks = task.Remarks,
                EngineerId = task.Engineer == null ? null : task.Engineer.Id,
                IsActive = true
            });
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

        if (_bl.GetProjectStatus() == BO.ProjectStatus.InExecution)
            throw new BO.BlDeletionImpossibleException("Can not delete task after the project start date was declared");

        IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll(item => item.DependsOnTask == id);
        if (dependencies.Any(item => Read(item.DependentTask).Status != BO.Status.Done))
            throw new BO.BlDeletionImpossibleException($"There are tasks that depend on Task with ID={id}");
                 
        try
        {
            DO.Task? doTask = _dal.Task.Read(id);
            if (doTask!.IsActive)
            {
                _dal.Task.Update(new DO.Task()
                {
                    Id = doTask.Id,
                    Complexity = doTask.Complexity,
                    CreatedAtDate = doTask.CreatedAtDate,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    StartDate = doTask.StartDate,
                    ScheduledDate = doTask.ScheduledDate,
                    CompleteDate = doTask.CompleteDate,
                    Deliverables = doTask.Deliverables,
                    Remarks = doTask.Remarks,
                    EngineerId = doTask.EngineerId,
                    IsActive = false
                });
            }
            else
            {
                //deleting all dependencies 
                List<DO.Dependency>? dependenciesToDelete = _dal.Dependency.ReadAll(item => item.DependentTask == id).ToList();
                foreach (var item in dependenciesToDelete)
                {
                    _dal.Dependency.Delete(item.Id);
                }

                _dal.Task.Delete(id);
            }
               
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist", ex);
        }
       
    }

    /// <summary>
    /// assigning scheduled start date to task
    /// </summary>
    /// <param name="id"> id of task </param>
    /// <param name="startDate"> scheduled start date </param>
    /// <exception cref="BO.BlDoesNotExistException"> if task does not exist in dal </exception>
    public void AssignScheduledDateToTask(int id, DateTime startDate)
    {
        CheckScheduledDate(id, startDate);
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        _dal.Task.Update(new DO.Task()
        {
            Id = task.Id,
            Complexity = task.Complexity,
            CreatedAtDate = task.CreatedAtDate,
            Description = task.Description,
            Alias = task.Alias,
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ScheduledDate = startDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            EngineerId = task.EngineerId
        });
    }

    /// <summary>
    /// Method that returns all tasks that requested engineer can be assigned to
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    /// <returns> list of tasks </returns>
    /// <exception cref="BO.BlDoesNotExistException"> if engineer does not exist </exception>
    public IEnumerable<BO.TaskInEngineer> ReadTasksForEngineer(int id)
    {
        DO.Engineer? engineer = _dal.Engineer.Read(id);
        if(engineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with id = {id} does not exist");

        return (from task in ReadAll(task=> task.Engineer == null && 
                (task.Dependencies == null || task.Dependencies.All(dependency => dependency.Status == BO.Status.Done)))
                where Read(task.Id).Complexity <= (BO.EngineerExperience)engineer.Level
                select new BO.TaskInEngineer() {Id = task.Id, Alias = task.Alias});
    }

    /// <summary>
    /// gets list of tasks that can be added as previous tasks/dependencies to task with given id
    /// </summary>
    /// <param name="id"> id of task to find dependencies for </param>
    /// <returns> list of dependencies to add </returns>
    public List<BO.TaskInList> GetDependenciesToAdd(int id)
    {
        BO.Task currentTask = Read(id);

        //finding all tasks except for current task and tasks that current task already depends on
        List<BO.Task> tasks = ReadAllFullTasks(item => item.Id != id
                && (currentTask.Dependencies == null ||
                !currentTask.Dependencies.Any(dep => dep.Id == item.Id))).ToList();

        //list of tasks that can cause circular dependencies
        List<BO.Task> tasksToRemove = new List<BO.Task>();

        //Checking circular dependency for all tasks
        foreach (var task in tasks)
        {
            if(task.Dependencies != null && task.Dependencies.Count > 0)
            {
                if(CheckCircularDependency(task.Dependencies, id))
                    tasksToRemove.Add(task);
            }                
        }

        //removing all tasks that need to be removed
        foreach (var task in tasksToRemove)
        {
            tasks.Remove(task);
        }

        return (from task in tasks
               select new BO.TaskInList()
               {
                   Id = task.Id,
                   Alias = task.Alias,
                   Description = task.Description,
                   Status = task.Status
               }).ToList();
    }

    /// <summary>
    /// reactivates task
    /// </summary>
    /// <param name="engineer"> task to recover </param>
    public void RecoverTask(BO.TaskInList task)
    {
        DO.Task? doTask = _dal.Task.Read(task.Id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Engineer with id = {task.Id} does not exist");
        _dal.Task.Update(new DO.Task()
        {
            Id = doTask.Id,
            Complexity = doTask.Complexity,
            CreatedAtDate = doTask.CreatedAtDate,
            Description = doTask.Description,
            Alias = doTask.Alias,
            RequiredEffortTime = doTask.RequiredEffortTime,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            EngineerId = doTask.EngineerId,
            IsActive = true
        });
    }

    #region Help methods
    /// <summary>
    /// Help method to check if scheduled start date is valid
    /// </summary>
    /// <param name="id"> id of task </param>
    /// <param name="startDate"> new/updated scheduled start date </param>
    /// <exception cref="BO.BlInvalidValueException"> if date is invalid </exception>
    void CheckScheduledDate(int id, DateTime startDate)
    {        
        BO.Task task = Read(id);
        IEnumerable<DO.Task?> depenedOnTasks = from item in task.Dependencies 
                               let doTask = _dal.Task.Read(item.Id)
                               where doTask.ScheduledDate == null || Read(doTask.Id).ForecastDate > startDate
                               select doTask;

        if (depenedOnTasks.Any())
            throw new BO.BlUpdatingImpossibleException($"Requested start date can not be assigned to this task because of previous tasks");
    }

    /// <summary>
    /// Help method to calculate task status
    /// </summary>
    /// <param name="doTask"> dal task entity </param>
    /// <returns> status of task </returns>
    BO.Status GetStatus(DO.Task doTask)
    {
        BO.Status currentStatus = BO.Status.Unscheduled;
        switch (doTask.ScheduledDate, doTask.StartDate, doTask.CompleteDate, doTask.RequiredEffortTime)
        {
            case var (scheduled, started, completed, time) when (scheduled != null && completed == null && scheduled + time < _bl.Clock)
                                                            || (GetDependencies(doTask.Id).Any(item => item.Status == BO.Status.Late)):
                currentStatus = BO.Status.Late;
                break;

            case var (scheduled, started, completed, time) when scheduled != null && started == null:
                currentStatus = BO.Status.Scheduled;
                break;

            case var (scheduled, started, completed, time) when started != null && completed == null:
                currentStatus = BO.Status.OnTrack;
                break;

            case var (scheduled, started, completed, time) when completed != null:
                currentStatus = BO.Status.Done;
                break;
        }

        return currentStatus;
    }

    
    /// <summary>
    /// private help method to create list of tasks that requested task depends on
    /// </summary>
    /// <param name="id"> id of dependent task</param>
    /// <returns> list of depends on tasks </returns>
    List<BO.TaskInList> GetDependencies(int id) 
    {
        return (from dependency in _dal.Dependency.ReadAll(item => item.DependentTask == id)
                let depTask = _dal.Task.Read(dependency.DependsOnTask)
                select new BO.TaskInList()
                {
                    Id = depTask.Id,
                    Description = depTask.Description,
                    Alias = depTask.Alias,
                    Status = GetStatus(depTask)
                }).ToList();
    }

    /// <summary>
    /// private help method to get name of engineer working on task
    /// </summary>
    /// <param name="doTask"> task </param>
    /// <returns> EngineerInTask entity which contains name and id of engineer </returns>
    BO.EngineerInTask? GetEngineerInTask(DO.Task doTask)
    {
        int engId = doTask.EngineerId ?? 0;
        DO.Engineer? engineer = null;
        BO.EngineerInTask? engineerInTask = null;
        if (engId != 0)
        {
            engineer = _dal.Engineer.Read(engId);
            engineerInTask = new BO.EngineerInTask() { Id = engineer.Id, Name = engineer.Name };
        }
        return engineerInTask;
    }

    /// <summary>
    /// Private help method to calculate forecast date
    /// </summary>
    /// <param name="doTask"> task </param>
    /// <returns> forecast date </returns>
    DateTime? GetForcast(DO.Task doTask)
    {
        DateTime? forcast = null;
        if (doTask.ScheduledDate != null && doTask.StartDate != null)
            forcast = (doTask.ScheduledDate > doTask.StartDate ? doTask.ScheduledDate : doTask.StartDate) + doTask.RequiredEffortTime;
        if (doTask.ScheduledDate != null && doTask.StartDate == null)
            forcast = doTask.ScheduledDate + doTask.RequiredEffortTime;
        return forcast;
    }

    /// <summary>
    /// help method that checks for circular dependencies
    /// </summary>
    /// <param name="tasks"> list of tasks to check </param>
    /// <param name="currentTaskId"> id of task </param>
    /// <returns> true if circular dependency exists </returns>
    bool CheckCircularDependency(List<BO.TaskInList> tasks, int currentTaskId)
    {
        foreach (var task in tasks)
        {
            if (task.Id == currentTaskId)
                return true;
            BO.Task fullTask = Read(task.Id);
            if (fullTask.Dependencies != null)
                return CheckCircularDependency(fullTask.Dependencies, currentTaskId);
        }

        return false;
    }

    /// <summary>
    /// help method to update dependencies, adding new and deleting 
    /// </summary>
    /// <param name="task"> task that is being updated </param>
    void updateDependencies(BO.Task task)
    {
        List<BO.TaskInList>? currentDependencies = Read(task.Id).Dependencies;
        List<BO.TaskInList>? newDependencies = task.Dependencies;
        if (currentDependencies == null)
            currentDependencies = new List<BO.TaskInList>();
        if (newDependencies == null)
            newDependencies = new List<BO.TaskInList>();

        var dependenciesToDelete = currentDependencies
                                   .Where(item => !newDependencies.Contains(item))
                                   .Select(item => _dal.Dependency.Read(dep => dep.DependsOnTask == item.Id))
                                   .ToList();

        var dependenciesToAdd = newDependencies
                                .Where(item => !currentDependencies.Contains(item))
                                .Select(item => item)
                                .ToList();

        dependenciesToDelete.ForEach(dependency => _dal.Dependency.Delete(dependency!.Id));
        dependenciesToAdd.ForEach(dependency => _dal.Dependency.Create(new DO.Dependency()
        {
            DependentTask = task.Id,
            DependsOnTask = dependency.Id
        }));
    }

    #endregion
}
