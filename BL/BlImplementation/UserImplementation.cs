namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly Bl _bl;
    internal UserImplementation(Bl bl) => _bl = bl;

    public void Create(BO.User? user)
    {
        if (user == null) throw new BO.BlNullPropertyException("User is null");
        if(user.Password == null) throw new BO.BlNullPropertyException("No password");
        try
        {
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
}