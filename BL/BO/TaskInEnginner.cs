namespace BO;

/// <summary>
/// help entity to use in engineer, containing id and alias of task
/// "Id" -  Main personal unique ID of the task
/// "Alias" - short, unique name for the task
/// </summary>
public class TaskInEnginner
{
    public int Id { get; init; }
    public string? Alias { get; init; }

    public override string ToString() => Tools.ToStringProperties(this);
}
