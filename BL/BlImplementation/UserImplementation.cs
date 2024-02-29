namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly Bl _bl;
    internal UserImplementation(Bl bl) => _bl = bl;

    /// <summary>
    /// adding new user to dal
    /// </summary>
    /// <param name="user"> user logic entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if user is null or has no password</exception>
    /// <exception cref="BO.BlAlreadyExistsException"> if user with given id already exists </exception>
    public void Create(BO.User? user)
    {
        if (user == null) throw new BO.BlNullPropertyException("User is null");
        if(user.Password == null) throw new BO.BlNullPropertyException("No password");
        try
        {
            user.Password = HashPassword(user.Password); //encrypting password
            _dal.User.Create(new DO.User()
            {
                Id = user.Id,
                Password = user.Password,
                Role = (DO.UserRole)user.Role
            });
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with ID={user!.Id} already exists", ex);
        }
    }

    /// <summary>
    /// reads user entity
    /// </summary>
    /// <param name="id"> id of user to read </param>
    /// <returns> user logic entity </returns>
    /// <exception cref="BO.BlDoesNotExistException"> if user with this id does not exist </exception>
    public BO.User Read(int id)
    {
        DO.User? user =  _dal.User.Read(id);
        if (user == null)
            throw new BO.BlDoesNotExistException($"User with Id: {id} does not exist");
        return new BO.User()
        {
            Id = id,
            Password = user.Password,
            Role = (BO.UserRole)user.Role
        };
    }

    /// <summary>
    /// reads all users
    /// </summary>
    /// <returns> collection of users </returns>
    public IEnumerable<BO.User> ReadAll()
    {
        return  from doUser in _dal.User.ReadAll()
                let boUser = new BO.User()
                {
                    Id = doUser.Id,
                    Password = doUser.Password,
                    Role = (BO.UserRole)doUser.Role
                }
                select boUser;                
    }

    /// <summary>
    /// updates user
    /// </summary>
    /// <param name="user"> updated user entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if user is null </exception>
    /// <exception cref="BO.BlDoesNotExistException"> if user with given id does not exist </exception>
    public void Update(BO.User? user)
    {
        try
        {
            if (user == null) throw new BO.BlNullPropertyException("User is null");
            if (user.Password == null) throw new BO.BlNullPropertyException("No password");
            _dal.User.Update(new DO.User()
            {
                Id = user.Id,
                Password = user.Password,
                Role = (DO.UserRole)user.Role
            });
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"User with ID={user!.Id} does Not exist", ex);
        }
    }

    /// <summary>
    /// deletes user from dal
    /// </summary>
    /// <param name="id"> id of user to delete </param>
    /// <exception cref="BO.BlDoesNotExistException"> user with given id does not exist </exception>
    public void Delete(int id)
    {
        try
        {
            _dal.User.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"User with ID={id} does Not exist", ex);
        }
    }

    /// <summary>
    /// hash function to encrypt password
    /// </summary>
    /// <param name="password"> password to encrypt </param>
    /// <returns> encrypted password </returns>
    public string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Compute hash from the password
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}