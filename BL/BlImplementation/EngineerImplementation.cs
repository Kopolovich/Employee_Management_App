namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(BO.Engineer? engineer)
    {
        try
        {
            if (engineer == null)
                throw new BlNullPropertyException("");
            if (engineer.Id <= 0 || engineer.Name == "" || engineer.Cost <= 0 || !CheckEmail(engineer.Email))
                throw new BlInvalidValueException("engineer with invalid values");

            _dal.Engineer.Create(new DO.Engineer(engineer.Id, (DO.EngineerExperience)engineer.Level, engineer.Email, engineer.Cost, engineer.Name));
        }
        catch (Exception ex)
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

    bool CheckEmail(string email)
    {
        // Basic regular expression for email validation
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        // Create a Regex object
        Regex regex = new Regex(pattern);
        // Use the IsMatch method to validate the email
        return regex.IsMatch(email);
    }
}
