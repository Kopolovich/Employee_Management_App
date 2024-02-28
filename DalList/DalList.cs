namespace Dal;
using DalApi;
using DO;
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

    public IUser User => new UserImplementation();

    public void Reset()
    {
        DataSource.Tasks.Clear();
        DataSource.Engineers.Clear();
        DataSource.Dependencies.Clear();
        DataSource.Users.Clear();

        DataSource.Config.ProjectStartDate = null;
        DataSource.Config.NextDependencyId = DataSource.Config.StartDependencyId;
        DataSource.Config.NextTaskId = DataSource.Config.StartTaskId;
    }
}
