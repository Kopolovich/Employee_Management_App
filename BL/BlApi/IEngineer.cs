namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// adding new engineer to data 
    /// </summary>
    /// <param name="engineer"> logic engineer entity</param>
    public void Create(BO.Engineer? engineer);

    
    /// <summary>
    /// read engineer
    /// </summary>
    /// <param name="id"> id of requested engineer</param>
    /// <returns> logic engineer entity </returns>
    public BO.Engineer? Read(int id);


    /// <summary>
    /// reads list of engineers
    /// </summary>
    /// <param name="filter">optional filter</param>
    /// <returns> collection of logic engineer entities </returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null);


    /// <summary>
    /// updating engineer
    /// </summary>
    /// <param name="engineer"> updated engineer</param>
    public void Update(BO.Engineer? engineer);


    /// <summary>
    /// deleting engineer from data
    /// </summary>
    /// <param name="id"> id of requested engineer </param>
    public void Delete(int id);
}
