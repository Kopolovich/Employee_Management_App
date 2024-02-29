using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";

    /// <summary>
    /// adding new User to XML file
    /// </summary>
    /// <param name="item">reference to new item to add</param>
    /// <returns>Id of new User</returns>
    public int Create(User item)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize

        if (Read(item.Id) != null)
            throw new DalAlreadyExistsException($"User with ID={item.Id} already exists");

        users.Add(item);

        XMLTools.SaveListToXMLSerializer(users, s_users_xml); //serialize

        return item.Id;
    }

    /// <summary>
    /// deletes requested User from XML file
    /// </summary>
    /// <param name="id">id of User to delete</param>
    /// <exception cref="DalDoesNotExistException">if requested User not found </exception>
    public void Delete(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize

        User? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"User with ID={id} does Not exist");
        else
            users.Remove(found);

        XMLTools.SaveListToXMLSerializer(users, s_users_xml); //serialize
    }

    /// <summary>
    /// retrieves requested User from XML file
    /// </summary>
    /// <param name="id">id of User to retrieve</param>
    /// <returns>retrieved User</returns>
    public User? Read(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize
        return users.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// retrieves requested User by filter from XML file
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in list that matches the filter</returns>
    public User? Read(Func<User, bool> filter)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize
        return users.FirstOrDefault(filter);
    }

    /// <summary>
    /// retrieves collection of Users from XML file
    /// </summary>
    /// <returns>copy of collection of Users</returns>
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize

        if (filter == null)
            return users.Select(item => item);
        else
            return users.Where(filter);
    }

    /// <summary>
    /// updates existing User from XML file
    /// </summary>
    /// <param name="item">updated User</param>
    /// <exception cref="DalDoesNotExistException">if requested User not found </exception>
    public void Update(User item)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml); //deserialize

        User? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"User with ID={item.Id} does Not exist");
        else
        {
            users.Remove(found);
            users.Add(item);
        }

        XMLTools.SaveListToXMLSerializer(users, s_users_xml); //serialize
    }
}
