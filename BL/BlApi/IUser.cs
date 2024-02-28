namespace BlApi;

public interface IUser
{
    /// <summary>
    /// adding new task to data 
    /// </summary>
    /// <param name="task"> logic task entity</param>
    public void Create(BO.User? user);


    /// <summary>
    /// read task
    /// </summary>
    /// <param name="id"> id of requested task</param>
    /// <returns> logic task entity </returns>
    public BO.User Read(int id);


    /// <summary>
    /// reads list of tasks
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic task entities </returns>
    public IEnumerable<BO.User> ReadAll();


    /// <summary>
    /// updating task
    /// </summary>
    /// <param name="task"> updated task </param>
    public void Update(BO.User? user);

    /// <summary>
    /// deleting task from data
    /// </summary>
    /// <param name="id"> id of requested task </param>
    public void Delete(int id);
}

