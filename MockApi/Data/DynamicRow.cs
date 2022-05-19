using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MockApi.Data;

public class DynamicRow
{
    public List<Column> Columns = new();
        
    public int RowID;

    /// <summary>
    ///  Check if their is a matching colum/value pair in this row
    /// </summary>
    /// <param name="column">name of column to check</param>
    /// <param name="forValue">value to check against</param>
    /// <returns>returns true on matching column/value, false if no match</returns>
    public bool ColumnMatches(string column, string forValue)
    {
        var col = Columns.Find(x => x.Key == column);
        if (col != null)
        {
            var match = col.Value == forValue;
            return match;
        }

        return false;
    }
    /// <summary>
    /// Converts column to a json object
    /// </summary>
    /// <returns>a string containing the json representation of the column</returns>
    public string ToJson()
    {
        var json = $"{{ \"id\" : \"{RowID}\",\n";
        foreach (var col in Columns)
        {
            json += $"\"{col.Key}\":\"{col.Value}\",";
        }

        // remove trailing commas as the formatter dont like them
        json = json.TrimEnd(',');
        
        json += "}";
        return json;
    }

    /// <summary>
    /// Add a single column to the end of the row, initializes column with provided value, if no value provided then it will be a default value
    /// </summary>
    /// <param name="name">name of column to add</param>
    /// <param name="value">the value to initialize the column with, defaults to null</param>
    public void AddColumn(string name, string? value = null)
    {
        var column = new Column(name)
        {
            Value = value
        };

        // add column
        Columns.Add(column);
    }

    /// <summary>
    /// Update the rows column names with a list of new names
    /// </summary>
    /// <param name="newNames">List of new names</param>
    public void RenameColumns(IEnumerable<string> newNames)
    {
        // cast IEnumerable to string so i can use indexes on it
        var names = newNames.ToArray();
        
        for (int i = 0; i < newNames.Count(); i++)
        {
            Columns[i].Key = names[i];
        }
        
    }

    public void DeleteColumns(IEnumerable<string> columnName)
    {
        Columns.RemoveAll(c => columnName.Contains(c.Key));
    }
    

    /// <summary>
    /// Updates a rows column with the new value, this method is called via the API not the UI itself
    /// </summary>
    /// <param name="column">The name of the column</param>
    /// <param name="value">The name of the key</param>
    public void UpdateColumnApi(string column, string value)
    {
        // select column that matches key and update value
        foreach (var c in Columns.Where(c => c.Key == column))
        {
            c.Value = value;
        }
        
    }

    /// <summary>
    /// Adds a list of columns to this row, each column is initialized with it's default value
    /// </summary>
    /// <param name="columnNames">List of column names to initialize</param>
    public void AddColumnsNoValues(IEnumerable<string> columnNames)
    {
        foreach (var name in columnNames)
        {
            Columns.Add(new Column(name));
        }
    }
}