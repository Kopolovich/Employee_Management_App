namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
/// Implementation of CRUD methods for Engineer entity
/// </summary>
internal class UserImplementation : IUser
{
    /// <summary>
    /// adding new Engineer to collection
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new Engineer</returns>
    /// <exception cref="Exception">if requested Engineer already exists</exception>
    public int Create(User item)
    {
        if (Read(item.Id) != null)
            throw new DalAlreadyExistsException($"User with ID={item.Id} already exists");
        else
        {
            DataSource.Users.Add(item);
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
        User? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"User with ID={id} does not exist");
        else
            DataSource.Users.Remove(found);
    }

    /// <summary>
    /// retrievs requested Engineer
    /// </summary>
    /// <param name="id">id of Engineer to retrieve</param>
    /// <returns>retrieved Engineer</returns>
    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(x => x.Id == id);
    }


    /// <summary>
    /// retrievs requested engineer by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in collection that matches the filter</returns>
    public User? Read(Func<User, bool> filter)
    {
        return DataSource.Users.FirstOrDefault(filter);
    }


    /// <summary>
    /// retreives collection of Engineers
    /// </summary>
    /// <returns>copy of collection of Engineers</returns>
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Users.Select(item => item);
        else
            return DataSource.Users.Where(filter);
    }


    /// <summary>
    /// updates existing Engineer
    /// </summary>
    /// <param name="item">updated Engineer</param>
    /// <exception cref="Exception">if requested Engineer not found </exception>
    public void Update(User item)
    {
        User? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"User with ID={item.Id} does Not exist");
        else
        {
            DataSource.Users.Remove(found);
            DataSource.Users.Add(item);
        }
    }
}