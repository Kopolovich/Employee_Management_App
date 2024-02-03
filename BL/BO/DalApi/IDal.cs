using DO;

namespace DalApi;

/// <summary>
/// An interface that represents the data layer and contains features of the sub-interfaces
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    ITask Task { get; }
    IDependency Dependency { get; }

    /// <summary>
    /// reset method to empty all lists/xml files
    /// </summary>
    void Reset() 
    {
        IEnumerable<DO.Task?> tasks = Task.ReadAll();
        if (tasks.Count() != 0)
        {
            foreach (var task in tasks)
                Task.Delete(task.Id);
        }

        IEnumerable<Dependency?> dependencies = Dependency.ReadAll();
        if (dependencies.Count() != 0)
        {
            foreach (var dependency in dependencies)
                Dependency.Delete(dependency.Id);
        }

        IEnumerable<Engineer?> engineers = Engineer.ReadAll();
        if (engineers.Count() != 0)
        {
            foreach (var engineer in engineers)
                Engineer.Delete(engineer.Id);
        }

        
    }
}
