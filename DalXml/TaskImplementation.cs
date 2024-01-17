namespace Dal;
using DalApi;
using DO;
using System.Data.Common;

/// <summary>
/// implementing Task CRUD methods using XMLSERIALIZER class
/// in beggining of each method the data is loaded from XML file into List
/// if changes were made, the List is saved to XML file
/// </summary>
internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// adding new Task to XML file
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new Task</returns>
    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize

        int newId = Config.NextTaskId;
        Task newTask = item with { Id = newId };
        tasks.Add(newTask);

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml); //serialize

        return newId;
    }

    /// <summary>
    /// deletes requested Task from XML file
    /// </summary>
    /// <param name="id">id of Task to delete</param>
    /// <exception cref="DalDoesNotExistException">if requested Task not found </exception>
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize

        Task? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        else
            tasks.Remove(found);

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml); //serialize
    }

    /// <summary>
    /// retrievs requested Task from XML file
    /// </summary>
    /// <param name="id">id of Task to retrieve</param>
    /// <returns>retrieved Task</returns>
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize
        return tasks.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// retrievs requested task by filter from XML file
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in list that matches the filter</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize
        return tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// retreives collection of Tasks from XML file
    /// </summary>
    /// <returns>copy of collection of Tasks</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize

        if (filter == null)
            return tasks.Select(item => item);
        else
            return tasks.Where(filter);
    }

    /// <summary>
    /// updates existing Task from XML file
    /// </summary>
    /// <param name="item">updated Task</param>
    /// <exception cref="DalDoesNotExistException">if requested Task not found </exception>
    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize

        Task? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        else
        {
            tasks.Remove(found);
            tasks.Add(item);
        }

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml); //serialize
    }

    /// <summary>
    /// help method to find id of task by description
    /// </summary>
    /// <param name="description">description of task to search for</param>
    /// <returns>id of requested task</returns>
    public int? FindId(string description)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //deserialize

        Task? task = tasks.FirstOrDefault(x => x.Description == description);
        if (task == null)
            return null;
        return task.Id;
    }
}
