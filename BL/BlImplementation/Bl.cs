namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public void CreateProjectSchedule(List<BO.Task> tasks, DateTime projectStartDate)
    {
        tasks.ForEach(task => Task.Create(task));

        List<BO.TaskInList> doTasks = Task.ReadAll().ToList();
        doTasks.ForEach(task => Task.AssignScheduledDateToTask(task.Id, GetEarliestDate(Task.Read(task.Id), projectStartDate)));
        
        Dal.Config.ProjectStartDate = projectStartDate;
    }

    DateTime GetEarliestDate(BO.Task task, DateTime projectStartDate)
    {
        if (task.Dependencies == null)
            return projectStartDate;
        
        var previosTasks = from dependency in task.Dependencies
                           select Task.Read(dependency.Id);

        if (previosTasks.Any(task => task.ScheduledDate == null))
            throw new BO.BlAssignmentImpossibleException("Can not assign scheduled start date to task if previos tasks dont have a scheduled start date yet");

        return (DateTime)previosTasks.MaxBy(task => task.ScheduledDate)!.ScheduledDate!;

    }

    //public ProjectStatus GetProjectStatus()
    //{
    //    if (Dal.Config.ProjectStartDate == null)
    //        return ProjectStatus.InPlanning;
    //    else return ProjectStatus.InExecution;
    //}
}
