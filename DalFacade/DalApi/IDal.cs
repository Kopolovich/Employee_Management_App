namespace DalApi;

/// <summary>
/// An interface that represents the data layer and contains features of the sub-interfaces
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    ITask Task { get; }
    IDependency Dependency { get; }
}
