namespace Dal;
using DalApi;
using DO;

/// <summary>
/// Implementation of CRUD methods of Engineer entity
/// </summary>
public class EngineerImplementation : IEngineer
{
    /// <summary>
    /// adding new Engineer to list
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new Engineer</returns>
    /// <exception cref="NotImplementedException">if requested Engineer not found</exception>
    public int Create(Engineer item)
    {

        Engineer? found = DataSource.Engineers.Find(x => x.Id == item.Id);
        if (found == null)
            throw new NotImplementedException();
        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
        
    }

    /// <summary>
    /// deletes requested Engineer from list
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
    /// <exception cref="NotImplementedException">if requested Engineer not found </exception>
    public void Delete(int id)
    {
        Engineer? found = DataSource.Engineers.Find(x => x.Id == id);
        if (found == null)
            throw new NotImplementedException();
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
        return DataSource.Engineers.Find(x => x.Id == id);
    }


    /// <summary>
    /// retreives list of Engineers
    /// </summary>
    /// <returns>copy of list of Engineers</returns>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    /// <summary>
    /// updates existing Engineer
    /// </summary>
    /// <param name="item">updated Engineer</param>
    /// <exception cref="NotImplementedException">if requested Engineer not found </exception>
    public void Update(Engineer item)
    {
        Engineer? found = DataSource.Engineers.Find(x => x.Id == item.Id);
        if (found == null)
            throw new NotImplementedException();
        else
        {
            DataSource.Engineers.Remove(found);
            DataSource.Engineers.Add(item);
        }
    }
}
