namespace BlApi;

public interface ITask
{
    /// <summary>
    /// adding new task to data 
    /// </summary>
    /// <param name="task"> logic task entity</param>
    public void Create(BO.Task? task);


    /// <summary>
    /// read task
    /// </summary>
    /// <param name="id"> id of requested task</param>
    /// <returns> logic task entity </returns>
    public BO.Task Read(int id);


    /// <summary>
    /// reads list of partial tasks
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic taskInList entities </returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// reads list of full tasks
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic task entities </returns>
    public IEnumerable<BO.Task> ReadAllFullTasks(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// reads list of not active tasks
    /// </summary>
    /// <returns> collection of logic taskInList entities </returns>
    public IEnumerable<BO.TaskInList> ReadAllNotActive();

    /// <summary>
    /// updating task
    /// </summary>
    /// <param name="task"> updated task </param>
    public void Update(BO.Task? task);

    /// <summary>
    /// deleting task from data
    /// </summary>
    /// <param name="id"> id of requested task </param>
    public void Delete(int id);

    /// <summary>
    /// assigning scheduled start date to task
    /// </summary>
    /// <param name="id"> id of task </param>
    /// <param name="startDate"> scheduled start date </param>
    public void AssignScheduledDateToTask(int id, DateTime startDate);

    /// <summary>
    /// Method that returns all tasks that requested engineer can be assigned to
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    /// <returns> list of tasks </returns>
    /// <exception cref="BO.BlDoesNotExistException"> if engineer does not exist </exception>
    public IEnumerable<BO.TaskInEngineer> ReadTasksForEngineer(int id);

    /// <summary>
    /// gets list of tasks that can be added as previous tasks/dependencies to task with given id
    /// </summary>
    /// <param name="id"> id of task to find dependencies for </param>
    /// <returns> list of dependencies to add </returns>
    public List<BO.TaskInList> GetDependenciesToAdd(int id);

    /// <summary>
    /// reactivates task
    /// </summary>
    /// <param name="engineer"> task to recover </param>
    public void RecoverTask(BO.TaskInList task);
}
