namespace Dal;
using DalApi;

/// <summary>
/// Implements the IDal interface by initializing the sub-interfaces in the access classes
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
