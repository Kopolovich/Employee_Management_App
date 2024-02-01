namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
/// Implementation of CRUD methods for Task entity
/// </summary>
internal class TaskImplementation : ITask
{
    /// <summary>
    /// adding new Task to collection
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new Task</returns>
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newId };
        DataSource.Tasks.Add(newTask);
        return newId;
    }

    /// <summary>
    /// deletes requested Task from collection
    /// </summary>
    /// <param name="id">id of Task to delete</param>
    /// <exception cref="Exception">if requested Task not found </exception>
    public void Delete(int id)
    {
        Task? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        else
            DataSource.Tasks.Remove(found);
    }

    /// <summary>
    /// retrievs requested Task
    /// </summary>
    /// <param name="id">id of Task to retrieve</param>
    /// <returns>retrieved Task</returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// retrievs requested task by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in list that matches the filter</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// retreives collection of Tasks
    /// </summary>
    /// <returns>copy of collection of Tasks</returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    /// <summary>
    /// updates existing Task
    /// </summary>
    /// <param name="item">updated Task</param>
    /// <exception cref="Exception">if requested Task not found </exception>
    public void Update(Task item)
    {
        Task? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        else
        {
            DataSource.Tasks.Remove(found);
            DataSource.Tasks.Add(item);
        }
    }

    /// <summary>
    /// help method to find id of task by description
    /// </summary>
    /// <param name="description">description of task to search for</param>
    /// <returns>id of requested task</returns>
    public int? FindId(string description)
    {
        Task? task = DataSource.Tasks.FirstOrDefault(x => x.Description == description);
        if (task == null)
            return null;
        return task.Id;
    }
}



