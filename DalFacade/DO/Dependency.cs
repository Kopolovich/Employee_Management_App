namespace DO;
/// <summary>
/// Dependency entity represents an dependency with all its props
/// </summary>
/// <param name="Id">Main personal unique ID of the dependency</param>
/// <param name="DependentTask">id of dependent task</param>
/// <param name="DependsOnTask">id of previos task that is dependent on</param>
public record Dependency
(
  int Id,
  int DependentTask,
  int DependsOnTask

)
{
    public Dependency() : this(0, 0, 0) { } //empty ctor
}
