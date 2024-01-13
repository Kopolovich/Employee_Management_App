namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
/// Implementation of CRUD methods for Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// adding new Engineer to collection
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new Engineer</returns>
    /// <exception cref="Exception">if requested Engineer already exists</exception>
    public int Create(Engineer item)
    {
        if (Read(item.Id) != null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }        
    }

    /// <summary>
    /// deletes requested Engineer from collection
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
    /// <exception cref="Exception">if requested Engineer not found </exception>
    public void Delete(int id)
    {
        Engineer? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        else
            DataSource.Engineers.Remove(found);
    }

    /// <summary>
    /// retrievs requested Engineer
    /// </summary>
    /// <param name="id">id of Engineer to retrieve</param>
    /// <returns>retrieved Engineer</returns>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(x => x.Id == id);
    }


    /// <summary>
    /// retrievs requested engineer by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in collection that matches the filter</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }


    /// <summary>
    /// retreives collection of Engineers
    /// </summary>
    /// <returns>copy of collection of Engineers</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }


    /// <summary>
    /// updates existing Engineer
    /// </summary>
    /// <param name="item">updated Engineer</param>
    /// <exception cref="Exception">if requested Engineer not found </exception>
    public void Update(Engineer item)
    {
        Engineer? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        else
        {
            DataSource.Engineers.Remove(found);
            DataSource.Engineers.Add(item);
        }
    }
}
