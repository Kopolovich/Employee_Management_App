namespace DO;
/// <summary>
/// Task entity represents an task with all its props
/// </summary>
/// <param name="Id"> Main personal unique ID of the task</param>
/// <param name="Complexity"> the minimum engineer level that can work on the task</param>
/// <param name="Alias"> short, unique name for the task</param>
/// <param name="Description"> description of the task</param>
/// <param name="CreatedAtDate">date that task was created by the administrator</param>
/// <param name="RequiredEffortTime">number or work days required to complete the task</param>
/// <param name="IsMilestone">represents if milestone</param>
/// <param name="StartDate">when an engineer begins the actual work</param>
/// <param name="ScheduledDate">planned date to start working on task</param>
/// <param name="DeadlineDate">deadline to complete task so project doesnt fail </param>
/// <param name="CompleteDate">date that engineer reports that he finished working on task</param>
/// <param name="Deliverables"> task result</param>
/// <param name="Remarks">optinal remarks on task</param>
/// <param name="EngineerId">id of engineer working on task</param>
public record Task
(
    int Id,
    EngineerExperience Complexity,
    DateTime CreatedAtDate,
    string? Alias = null,
    string? Description = null,
    TimeSpan? RequiredEffortTime = null,
    bool? IsMilestone = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null
)
{
    public Task() : this(0, EngineerExperience.Beginner, DateTime.Now) { } //empty ctor
}
