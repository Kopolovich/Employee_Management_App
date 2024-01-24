namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(BO.Engineer? engineer)
    {
        try
        {

        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer? engineer)
    {
        throw new NotImplementedException();
    }
}
