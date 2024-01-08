namespace DalApi;
using DO;
public interface ITask : ICrud <Task>
{
    int? FindId(string description); //help method to find id of task by description
}
