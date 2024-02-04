namespace Dal;
using DalApi;
using System.Diagnostics;

/// <summary>
/// Implements the IDal interface by initializing the sub-interfaces in the access classes
/// </summary>
sealed internal class DalXml : IDal
{
    //public static IDal Instance => new DalXml();
    //private DalXml() { }

    private static readonly Lazy<DalXml> lazy =
      new Lazy<DalXml>(() => new DalXml());

    public static DalXml Instance { get { return lazy.Value; } }

    private DalXml() { }


    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
