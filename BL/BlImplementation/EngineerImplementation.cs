namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    private BlImplementation.TaskImplementation _Itask = new BlImplementation.TaskImplementation();

    /// <summary>
    /// adding new engineer to dal
    /// </summary>
    /// <param name="engineer"> engineer logic entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if recieved engineer is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of engineer's props contain invalid values </exception>
    /// <exception cref="BO.BlAlreadyExistsException"> if engineer with this Id already exists </exception>
    public void Create(BO.Engineer? engineer)
    {
        try
        {
            if (engineer == null)
                throw new BO.BlNullPropertyException("Enigneer is null");
            if (engineer.Id <= 0 || engineer.Name == "" || engineer.Cost <= 0 || !CheckEmail(engineer.Email!))
                throw new BO.BlInvalidValueException("Engineer with invalid values");

            //adding engineer using dal Create method
            _dal.Engineer.Create(new DO.Engineer(engineer.Id, (DO.EngineerExperience)engineer.Level, engineer.Email, engineer.Cost, engineer.Name));
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer!.Id} already exists", ex);
        }
    }

    /// <summary>
    /// reads engineer
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    /// <returns> logic engineer entity </returns>
    /// <exception cref="BO.BlDoesNotExistException"> if engineer with given id does not exist </exception>
    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist");

        IEnumerable<DO.Task> dTasks = _dal.Task.ReadAll(item => item.EngineerId == id);

        DO.Task? currentTask = (from DO.Task dTask in dTasks
                               where _Itask.Read(dTask.Id).Status == BO.Status.OnTrack
                               select dTask).FirstOrDefault();                               

        BO.TaskInEnginner ? taskInEnginner = null;
        if (currentTask != null)
            taskInEnginner = new BO.TaskInEnginner() { Id = currentTask.Id, Alias = currentTask.Alias };
        
        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = taskInEnginner
        };
    }
    
    /// <summary>
    /// reads collection of all engineers
    /// </summary>
    /// <param name="filter"> optional filter to filter engineers </param>
    /// <returns> collection of logic engineer entities </returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        return (
                from DO.Engineer doEngineer in _dal.Engineer.ReadAll(filter)
                let boEngineer = Read(doEngineer.Id) //using Read method to create logic entity
                select boEngineer 
                );
    }

    /// <summary>
    /// updates existing engineer
    /// </summary>
    /// <param name="engineer"> updated logic engineer entity </param>
    /// <exception cref="BO.BlNullPropertyException"> if recieved engineer is null </exception>
    /// <exception cref="BO.BlInvalidValueException"> if one or more of updated engineer's props contain invalid values </exception>
    /// <exception cref="BO.BlDoesNotExistException"> if engineer does not exist in dal </exception>
    /// <exception cref="BO.BlAlreadyExistsException"> the Create method that is called in dal Update method might throw an exception </exception>
    public void Update(BO.Engineer? engineer)
    {
        try
        {
            if (engineer == null)
                throw new BO.BlNullPropertyException("Enigneer is null");
            if (engineer.Id <= 0 || engineer.Name == "" || engineer.Cost <= 0 || !CheckEmail(engineer.Email!))
                throw new BO.BlInvalidValueException("Engineer with invalid values");

            //making sure the updated level is not lower than current level
            DO.Engineer? doEngineer= _dal.Engineer.Read(engineer.Id);
            if (doEngineer == null)
                throw new BO.BlDoesNotExistException($"Engineer with ID={engineer!.Id} does Not exist");
            if (doEngineer.Level > (DO.EngineerExperience)engineer.Level)
                throw new BO.BlInvalidValueException("It is not possible to lower the experience level of an engineer");

            _dal.Engineer.Update(new DO.Engineer(engineer.Id, (DO.EngineerExperience)engineer.Level, engineer.Email, engineer.Cost, engineer.Name));
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer!.Id} already exists", ex);
        }
        catch(DO.DalDoesNotExistException ex) 
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer!.Id} does Not exist", ex);
        }
    }

    /// <summary>
    /// deletes engineer
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    /// <exception cref="BO.BlDeletionImpossibleException"> if engineer is currently working on a task </exception>
    /// <exception cref="BO.BlDoesNotExistException"> if requested engineer does not exist </exception>
    public void Delete(int id)
    {
        //if engineer does not exist in dal the Read method throws an exception
        BO.Engineer engineer = Read(id);

        //checking if there is a task that engineer is currently working on
        if(engineer.Task != null)
        {
            BO.Task boTask = _Itask.Read(engineer.Task.Id);
            if (boTask.Status == BO.Status.OnTrack)
                throw new BO.BlDeletionImpossibleException("Can not delete engineer that is currently working on a task");
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exist", ex);
        }
        
    }

    /// <summary>
    /// private help method to check if email address is valid
    /// </summary>
    /// <param name="email"> email address </param>
    /// <returns> true if email is valid, else false </returns>
    bool CheckEmail(string email)
    {
        if (email == null)
            return false;
        // Basic regular expression for email validation
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        // Create a Regex object
        Regex regex = new Regex(pattern);
        // Use the IsMatch method to validate the email
        return regex.IsMatch(email);
    }
}
