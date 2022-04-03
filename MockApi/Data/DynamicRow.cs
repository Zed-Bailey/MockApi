using System.Dynamic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MockApi.Data;

// https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=net-6.0
public class DynamicRow : DynamicObject
{
    private Dictionary<string, object?> _dictionary = new();
    
    // This property returns the number of elements
    // in the inner dictionary.
    public int Count => _dictionary.Count;

    public object ValueFor(string key)
    {
        if (_dictionary.ContainsKey(key))
        {
            return _dictionary[key] ?? "";
        }

        return "";
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

    public bool Update(string key, object value)
    {
        _dictionary[key] = value;
        return true;
    }
    // If you try to get a value of a property
    // not defined in the class, this method is called.
    public override bool TryGetMember(
        GetMemberBinder binder, out object? result)
    {
        // Converting the property name to lowercase
        // so that property names become case-insensitive.
        string name = binder.Name.ToLower();

        // If the property name is found in a dictionary,
        // set the result parameter to the property value and return true.
        // Otherwise, return false.
        return _dictionary.TryGetValue(name, out result);
    }

    // If you try to set a value of a property that is
    // not defined in the class, this method is called.
    public override bool TrySetMember(
        SetMemberBinder binder, object? value)
    {
        // Converting the property name to lowercase
        // so that property names become case-insensitive.
        _dictionary[binder.Name.ToLower()] = value;

        // You can always add a value to a dictionary,
        // so this method always returns true.
        return true;
    }
    
    
    /// <summary>
    /// Overriding Equals is essential for use with Select and Table because they use HashSets internally
    /// </summary>
    public override bool Equals(object obj) => object.Equals(GetHashCode(), obj?.GetHashCode());

    /// <summary>
    /// Overriding GetHashCode is essential for use with Select and Table because they use HashSets internally
    /// </summary>
    public override int GetHashCode() => _dictionary.GetHashCode();
}