namespace BO;

/// <summary>
/// help entity to use in task, containing only id and alias of milestone
/// "Id" -  Main personal unique ID of the milestone
/// "Alias" - short, unique name for the milestone
/// </summary>
public class MilestoneInTask
{
    public int Id { get; init; }
    public string? Alias { get; set; }
}
