using DO;

namespace DalApi;

/// <summary>
/// An interface that represents the data layer and contains features of the sub-interfaces
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    ITask Task { get; }
    IDependency Dependency { get; }
    IUser User { get; }

    /// <summary>
    /// reset method to empty all lists/xml files
    /// </summary>
    void Reset(); 

    //property for project start date
    DateTime? StartDate { get; set; }
 
}
