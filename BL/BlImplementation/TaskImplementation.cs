namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

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

        if(task.Dependencies != null)
        {
             task.Dependencies.ForEach(depTask => _dal.Dependency.Create(new DO.Dependency() 
             { DependentTask = task.Id, DependsOnTask = depTask.Id }));
        }
    }


    public BO.Task Read(int id)
    {
        DO.Task doTask = _dal.Task.Read(id);
        if(doTask == null) throw new BO.BlDoesNotExistException($"Task with ID={id} does not exist");

        IEnumerable <DO.Dependency>? dependencies = _dal.Dependency.ReadAll(item => item.DependentTask == id);

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
            engineerInTask = new EngineerInTask() { Id = engineer.Id, Name = engineer.Name };
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

    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {

        
        
        
        
        
        
        throw new NotImplementedException();
       
    }



    public void Update(BO.Task? task)
    {
        throw new NotImplementedException();
    }


    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

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
