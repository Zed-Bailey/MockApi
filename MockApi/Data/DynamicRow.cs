using System.Dynamic;
using System.Text.Json;

namespace MockApi.Data;

// https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=net-6.0
//TODO: clean this class up to just use the dictionary as i am not using most of the features that the dynamic object provides

public class DynamicRow
{
    public Dictionary<string, string?> _dictionary = new();
    
    // This property returns the number of elements
    // in the inner dictionary.
    public int Count => _dictionary.Count;

    /// <summary>
    /// Return the value for the passed in column key
    /// </summary>
    /// <param name="column">Name of column</param>
    /// <returns>The value in the column, null if no value is in column</returns>
    public string? ValueFor(string column)
    {
        return  _dictionary[column];
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(_dictionary);
    }

    /// <summary>
    /// Adds a new member to the row with a default value
    /// </summary>
    /// <param name="name">The name of the new member to add</param>
    public void AddNewMember(string name)
    {
        _dictionary[name] = null;
    }
    
    /// <summary>
    /// Initializes the row with all the members required with null
    /// </summary>
    /// <param name="keys">A List of key names to add</param>
    public void AddMembersNoValue(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            _dictionary[key] = null;
        }
    }

    public bool Update(string key, string value)
    {
        _dictionary[key] = value;
        return true;
    }
}