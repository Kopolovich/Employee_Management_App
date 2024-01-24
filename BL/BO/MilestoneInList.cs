namespace BO;

/// <summary>
/// help entity, containing only description, alias, created date, status and completion percentage of milestone
/// "Description" - description of the milestone
/// "Alias" - short, unique name for the milestone
/// "CreatedAtDate" - date that milestone was created by the administrator
/// "Status" - status of milestone
/// "CompletionPercentage" - percentage of completed tasks out of the tasks that the milestone is dependent on
/// </summary>
public class MilestoneInList
{
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime? CreatedDate { get; init; }
    public Status Status { get; set; }
    public double? CompletionPercentage { get; set; }
}
