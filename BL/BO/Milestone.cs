namespace BO;

/// <summary>
/// Milestone logic entity represents a milestone with all its props
/// "Id" - Main personal unique ID of the milestone
/// "Description" - description of the milestone
/// "Alias" - short, unique name for the milestone
/// "CreatedAtDate" - date that milestone was created by the administrator
/// "Status" - status of milestone
/// "ForecastDate" - estimated completion date
/// "DeadlineDate" - deadline to complete milestone so project doesnt fail
/// "CompleteDate" - date that all tasks that milestone depends on are completed
/// "CompletionPercentage" - percentage of completed tasks out of the tasks that the milestone is dependent on
/// "Remarks" - optinal remarks on milestone
/// "Dependencies" - list of dependencies
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Status Status { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public TaskInList? Dependencies { get; set; }

}
