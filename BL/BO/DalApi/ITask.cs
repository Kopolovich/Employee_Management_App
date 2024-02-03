namespace DalApi;
using DO;
/// <summary>
/// Task interface that inherits from generic interface - Icrud
/// </summary>
public interface ITask : ICrud <Task>
{
    int? FindId(string description); //help method to find id of task by description
}
