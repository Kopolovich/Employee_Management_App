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
    /// adding new Task to list
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
    /// deletes requested Task from list
    /// </summary>
    /// <param name="id">id of Task to delete</param>
    /// <exception cref="Exception">if requested Task not found </exception>
    public void Delete(int id)
    {
        Task? found = DataSource.Tasks.FirstOrDefault(x => x.Id == id);
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
        Task? temp = DataSource.Tasks.FirstOrDefault(x => x.Id == id);
        if (temp == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        return temp;
    }

    /// <summary>
    /// retrievs requested task by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in list that matches the filter</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        Task? temp = DataSource.Tasks.FirstOrDefault(filter);
        if (temp == null)
            throw new DalDoesNotExistException("Requested task does not exist");
        return temp;
    }

    /// <summary>
    /// retreives list of Tasks
    /// </summary>
    /// <returns>copy of list of Tasks</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
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
        Task? found = DataSource.Tasks.FirstOrDefault(x => x.Id == item.Id);
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



