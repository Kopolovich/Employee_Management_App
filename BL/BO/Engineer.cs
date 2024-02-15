namespace BO;

/// <summary>
/// Engineer logic entity with all its props
/// "Id" -  Main personal unique ID of the task
/// "Name" - name of engineer
/// "Email" - engineer's email address 
/// "Level" - engineer's level of experience
/// "Cost" - engineer's hourly salary 
/// "Task" - id and alias of task that engineer is currently working on
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double? Cost { get; set; }
    public TaskInEngineer? Task { get; set; }

    public override string ToString() => Tools.ToStringProperties(this);
}
