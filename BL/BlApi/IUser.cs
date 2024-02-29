namespace BlApi;

public interface IUser
{
    /// <summary>
    /// adding new user to data 
    /// </summary>
    /// <param name="user"> logic user entity</param>
    public void Create(BO.User? user);


    /// <summary>
    /// read user
    /// </summary>
    /// <param name="id"> id of requested user</param>
    /// <returns> logic user entity </returns>
    public BO.User Read(int id);


    /// <summary>
    /// reads list of users
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic user entities </returns>
    public IEnumerable<BO.User> ReadAll();


    /// <summary>
    /// updating user
    /// </summary>
    /// <param name="user"> updated user </param>
    public void Update(BO.User? user);

    /// <summary>
    /// deleting user from data
    /// </summary>
    /// <param name="id"> id of requested user </param>
    public void Delete(int id);

    /// <summary>
    /// hash function to encrypt password
    /// </summary>
    /// <param name="password"> password to encrypt </param>
    /// <returns> encrypted password </returns>
    public string HashPassword(string password);
}

