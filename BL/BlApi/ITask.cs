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
    /// reads list of tasks
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic task entities </returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);


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
}
