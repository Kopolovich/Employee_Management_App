namespace Dal;
using DalApi;

/// <summary>
/// Implements the IDal interface by initializing the sub-interfaces in the access classes
/// </summary>
sealed public class DalList : IDal
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
