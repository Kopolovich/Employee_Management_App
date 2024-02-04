namespace BO;
/// <summary>
/// Task logic entity represents a task with all its props
/// "Id" -  Main personal unique ID of the task
/// "Description" - description of the task
/// "Alias" - short, unique name for the task
/// "CreatedAtDate" - date that task was created by the administrator
/// "Status" - status of task
/// "Dependencies" - list of dependencies
/// "RequiredEffortTime" - number of work days required to complete the task
/// "StartDate" - when an engineer begins the actual work
/// "ScheduledDate" - planned date to start working on task
/// "ForecastDate" - estimated completion date
/// "CompleteDate" - date that engineer reports that he finished working on task
/// "Deliverables" - task result
/// "Remarks" - optinal remarks on task
/// "Engineer" - name and id of engineer assigned to task, if exists
/// "Complexity" - the minimum engineer level that can work on the task
/// </summary>
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public List<TaskInList>? Dependencies { get; set;}
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set;}
    public DateTime? ForecastDate { get; set; }
    public DateTime? CompleteDate {  get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience Complexity { get; set;}

    public override string ToString() => Tools.ToStringProperties(this);
}
