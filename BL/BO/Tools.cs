using System.Runtime.CompilerServices;

internal class Tools
{
    /// <summary>
    /// Help method for printing entities props
    /// </summary>
    /// <typeparam name="T"> type of entity </typeparam>
    /// <param name="obj"> sent entity </param>
    /// <returns> string with props ready for print </returns>
    public static string ToStringProperties<T>(T obj)
    {
        string str = "";
        var properties = typeof(T).GetProperties();

        //for each property, print the name and than the value
        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            //if the value itself is a collection, like dependencies
            if (value is IEnumerable<object> items)
            {
                str += $"{property.Name}:\n";
                foreach (var item in items)
                {
                    str += $"- {item}\n";
                }
            }
            if(value is BO.TaskInEngineer task)
            {
                str += $"{property.Name}: Id: {task.Id} Alias: {task.Alias}\n";
            }
            else
            {
                str += $"{property.Name}: ";

                //if (value != null && value.ToString()!.Length > 40)
                //{

                //    int splitIndex = value.ToString()!.LastIndexOf(' ', 40);

                //    string line1 = value.ToString()!.Substring(0, splitIndex);
                //    string line2 = value.ToString()!.Substring(splitIndex+1);
                //    str += (line1 + "\n" + line2 + "\n");
                //}
                //else
                    str += $"{value}\n";
            }
        }

        return str;
    }
}
