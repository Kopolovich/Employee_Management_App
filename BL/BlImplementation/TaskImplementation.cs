namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(BO.Task? task)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task? task)
    {
        throw new NotImplementedException();
    }
}
