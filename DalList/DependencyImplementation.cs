namespace Dal;
using DalApi;
using DO;
/// <summary>
/// Implementation of CRUD methods of Dependency entity
/// </summary>
public class DependencyImplementation : IDependency
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
    /// <exception cref="NotImplementedException">if requested dependency not found </exception>
    public void Delete(int id)
    {
        Dependency? found = DataSource.Dependencies.Find(x => x.Id == id);
        if (found==null)
            throw new NotImplementedException();
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
        return DataSource.Dependencies.Find(x => x.Id == id);
    }

    /// <summary>
    /// retreives list of dependencies
    /// </summary>
    /// <returns>copy of list of dependencies</returns>
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    /// <summary>
    /// updates existing dependency
    /// </summary>
    /// <param name="item">updated dependency</param>
    /// <exception cref="NotImplementedException">if requested dependency not found </exception>
    public void Update(Dependency item)
    {
        Dependency? found = DataSource.Dependencies.Find(x => x.Id == item.Id);
        if (found == null)
            throw new NotImplementedException();
        else
        {
            DataSource.Dependencies.Remove(found);
            DataSource.Dependencies.Add(item);
        }
    }
}
