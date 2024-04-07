namespace Dal;
using DalApi;
using DO;
using System.Data.SqlTypes;

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

    public IUser User => new UserImplementation();

    public DateTime? StartDate { get => DataSource.Config.ProjectStartDate; set => DataSource.Config.ProjectStartDate = value; }

    public void Reset()
    {
        DataSource.Tasks.Clear();
        DataSource.Engineers.Clear();
        DataSource.Dependencies.Clear();
        IEnumerable<User?> users = User.ReadAll(user => user.Role == UserRole.Engineer);
        if (users.Count() != 0)
        {
            foreach (var user in users)
                DataSource.Users.Remove(user);
        }

        DataSource.Config.ProjectStartDate = null;
        DataSource.Config.NextDependencyId = DataSource.Config.StartDependencyId;
        DataSource.Config.NextTaskId = DataSource.Config.StartTaskId;
    }
}
