namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;
using System.Data.Common;
using System.Reflection.Emit;
using System;

/// <summary>
/// implementing Engineer CRUD methods using XElement class
/// </summary>
internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// adding new Engineer to XML file
    /// </summary>
    /// <param name="item">refernce to new item to add</param>
    /// <returns>Id of new Engineer</returns>
    /// <exception cref="DalAlreadyExistsException">if requested Engineer already exists</exception>
    public int Create(Engineer item)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml); //Loading data from file into Xelement object
        if (Read(item.Id) != null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        else
        {
            XElement id = new XElement("Id", item.Id);
            XElement level = new XElement("Level", item.Level);
            XElement email = new XElement("Email", item.Email);
            XElement name = new XElement("Name", item.Name);

            engineerRoot.Add(new XElement("Engineer", id, level, email, name)); //adding to root new Xelement containig XElements for each field
            XMLTools.SaveListToXMLElement(engineerRoot, s_engineers_xml); //saving updated root to XML file
            return item.Id;
        }
    }

    /// <summary>
    /// deletes requested Engineer from XML file
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
    /// <exception cref="Exception">if requested Engineer not found </exception>
    public void Delete(int id)
    {
        XElement engineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml); //Loading data from file into Xelement object
        XElement? found = (from p in engineerRoot.Elements()
                           where Convert.ToInt32(p.Element("Id").Value) == id
                           select p).FirstOrDefault();

        if (found == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");

        found.Remove(); //removing requested item from root
        XMLTools.SaveListToXMLElement(engineerRoot, s_engineers_xml); //saving updated root to XML file
    }

    /// <summary>
    /// retrievs requested Engineer
    /// </summary>
    /// <param name="id">id of Engineer to retrieve</param>
    /// <returns>retrieved Engineer</returns>
    public Engineer? Read(int id)
    {
        XElement? found = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().FirstOrDefault(eng => (int)eng.Element("Id") == id);
        return found is null ? null : getEngineer(found);
    }


    /// <summary>
    /// retrievs requested engineer by filter
    /// </summary>
    /// <param name="filter">Func type delegate, boolian function to filter</param>
    /// <returns>first item in collection that matches the filter</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e=>getEngineer(e)).FirstOrDefault(filter);
    }


    /// <summary>
    /// retreives collection of Engineers
    /// </summary>
    /// <returns>copy of collection of Engineers</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)); //returns all items
        else
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)).Where(filter); //returns all items that match filter
    }

    /// <summary>
    /// updates existing Engineer
    /// </summary>
    /// <param name="item">updated Engineer</param>
    /// <exception cref="Exception">if requested Engineer not found </exception>
    public void Update(Engineer item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        else
        {
            Delete(item.Id); //Removing old item
            Create(item); //adding updated item
        }
    }

    /// <summary>
    /// help method
    /// converts XElement to engineer
    /// </summary>
    /// <param name="e">Xelement</param>
    /// <returns>Engineer object</returns>
    static Engineer getEngineer(XElement e)
    {
        return new Engineer()
        {
            Id = Convert.ToInt32(e.Element("Id").Value),
            Level = (EngineerExperience)e.ToEnumNullable<EngineerExperience>(e.Element("Level").Value),
            Email = e.Element("Email").Value ?? null,
            Name = e.Element("Name").Value ?? null,
        };
    }
}
