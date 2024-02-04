namespace BO;

/// <summary>
/// help entity for task, containing only id, description, alias and status
/// "Id" -  Main personal unique ID of the task
/// "Description" - description of the task
/// "Alias" - short, unique name for the task
/// "Status" - status of task
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public Status Status { get; set; }

    public override string ToString() => Tools.ToStringProperties(this);
}
