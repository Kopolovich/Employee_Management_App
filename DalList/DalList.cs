namespace Dal;
using DalApi;
using System.Data.SqlTypes;

/// <summary>
/// Implements the IDal interface by initializing the sub-interfaces in the access classes
/// </summary>
sealed internal class DalList : IDal
{
    //public static IDal Instance { get; } = new DalList();
    //private DalList() { }
    private static readonly Lazy<DalList> lazy =
    new Lazy<DalList>(() => new DalList());

    public static DalList Instance { get { return lazy.Value; } }

    private DalList() { }

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
