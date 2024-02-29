namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
/// Implementation of CRUD methods for User entity
/// </summary>
internal class UserImplementation : IUser
{
    /// <summary>
    /// adding new User to collection
    /// </summary>
    /// <param name="item">reference to new item to add</param>
    /// <returns>Id of new User</returns>
    /// <exception cref="DalAlreadyExistsException">if requested User already exists</exception>
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
    /// deletes requested User from collection
    /// </summary>
    /// <param name="id">id of User to delete</param>
    /// <exception cref="Exception">if requested User not found </exception>
    public void Delete(int id)
    {
        User? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"User with ID={id} does not exist");
        else
            DataSource.Users.Remove(found);
    }

    /// <summary>
    /// retrieves requested User
    /// </summary>
    /// <param name="id">id of User to retrieve</param>
    /// <returns> retrieved User </returns>
    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(x => x.Id == id);
    }


    /// <summary>
    /// retrieves requested User by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in collection that matches the filter</returns>
    public User? Read(Func<User, bool> filter)
    {
        return DataSource.Users.FirstOrDefault(filter);
    }


    /// <summary>
    /// retrieves collection of Users
    /// </summary>
    /// <returns>copy of collection of Users</returns>
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Users.Select(item => item);
        else
            return DataSource.Users.Where(filter);
    }

    /// <summary>
    /// updates existing User
    /// </summary>
    /// <param name="item">updated Users</param>
    /// <exception cref="Exception">if requested User not found </exception>
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