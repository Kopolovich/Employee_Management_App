namespace BO;

/// <summary>
/// help entity to use in task, contains id and name of engineer assigned to task
/// "Id" -  Main personal unique ID of the task
/// "Name" - name of engineer
/// </summary>
public class EngineerInTask
{
    public int Id { get; init; }
    public string? Name { get; set; }

    public override string ToString() => Tools.ToStringProperties(this);
}
