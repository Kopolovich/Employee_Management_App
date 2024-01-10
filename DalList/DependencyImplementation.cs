namespace Dal;
using DalApi;
using DO;
/// <summary>
/// Implementation of CRUD methods for Dependency entity
/// </summary>
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// adding new Dependency to list
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new dependency</returns>
    public int Create(Dependency item)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency newDependency = item with {Id = newId};
        DataSource.Dependencies.Add(newDependency);
        return newId;
    }

    /// <summary>
    /// deletes requested dependency from list
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    /// <exception cref=Exception">if requested dependency not found </exception>
    public void Delete(int id)
    {
        Dependency? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");
        else
            DataSource.Dependencies.Remove(found);        
    }

    /// <summary>
    /// retrievs requested dependency
    /// </summary>
    /// <param name="id">id of dependency to retrieve</param>
    /// <returns>retrieved dependency</returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// retrievs requested dependency by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in list that matches the filter</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    /// <summary>
    /// retreives list of dependencies
    /// </summary>
    /// <returns>copy of list of dependencies</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }

    /// <summary>
    /// updates existing dependency
    /// </summary>
    /// <param name="item">updated dependency</param>
    /// <exception cref="Exception">if requested dependency not found </exception>
    public void Update(Dependency item)
    {
        Dependency? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        else
        {
            DataSource.Dependencies.Remove(found);
            DataSource.Dependencies.Add(item);
        }
    }
}
