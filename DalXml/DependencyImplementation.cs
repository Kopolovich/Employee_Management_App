namespace Dal;
using DalApi;
using DO;
using System.Data.Common;

/// <summary>
/// implementing Dependency CRUD methods using XMLSERIALIZER class
/// in beggining of each method the data is loaded from XML file into List
/// if changes were made, the List is saved to XML file
/// </summary>
internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    /// <summary>
    /// adding new Dependency to XML file
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new dependency</returns>
    public int Create(Dependency item)
    {
        List<Dependency> dependencies= XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); //deserialize

        int newId = Config.NextDependencyId;
        Dependency newDependency = item with { Id = newId };
        dependencies.Add(newDependency);

        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml); //serialize

        return newId;
    }

    /// <summary>
    /// deletes requested dependency from XML file
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    /// <exception cref=DalDoesNotExistException">if requested dependency not found </exception>
    public void Delete(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); //deserialize

        Dependency? found = Read(id);
        if (found == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");
        else
            dependencies.Remove(found);

        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml); //serialize
    }

    /// <summary>
    /// retrievs requested dependency from XML file
    /// </summary>
    /// <param name="id">id of dependency to retrieve</param>
    /// <returns>retrieved dependency</returns>
    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); //deserialize
        return dependencies.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// retrievs requested dependency by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in XML file that matches the filter</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); //deserialize
        return dependencies.FirstOrDefault(filter);
    }

    /// <summary>
    /// retreives collection of dependencies from XML file
    /// </summary>
    /// <returns>copy of XML file of dependencies</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); //deserialize

        if (filter == null)
            return dependencies.Select(item => item);
        else
            return dependencies.Where(filter);
    }

    /// <summary>
    /// updates existing dependency in XML file
    /// </summary>
    /// <param name="item">updated dependency</param>
    /// <exception cref="DalDoesNotExistException">if requested dependency not found </exception>
    public void Update(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); //deserialize
        Dependency? found = Read(item.Id);
        if (found == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        else
        {
            dependencies.Remove(found);
            dependencies.Add(item);
        }

        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml); //serialize
    }
}
