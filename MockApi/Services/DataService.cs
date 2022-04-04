using System.Dynamic;
using MudBlazor;
using System.Collections.Generic;
using MockApi.Data;

namespace MockApi.Services;

public class DataService
{
    // https://www.oreilly.com/content/building-c-objects-dynamically/
    public readonly List<string> ColumnNames  = new();

    public List<DynamicRow> Rows { get; } = new ();
    
    public DataService()
    {
        ColumnNames = new List<string> { "test", "test 2", "omg another column"};
    }

    /// <summary>
    /// Check to see if a column exists
    /// </summary>
    /// <param name="name">name of the column to check</param>
    /// <returns>returns true if it exists. false otherwise</returns>
    public bool ColumnExists(string name)
    {
        return ColumnNames.Contains(name);
    }

    /// <summary>
    /// Add a new column to all the rows
    /// </summary>
    /// <param name="name">Name of the new column</param>
    /// <returns>false if a column already exists with that name</returns>
    public bool AddColumn(string name)
    {
        // check if column already exists
        if (ColumnExists(name)) return false;
        
        // add column name and update rows
        ColumnNames.Add(name);
        foreach (var row in Rows)
        {
            row.AddNewMember(name);
        }

        return true;
    }
    
    public void AddRow()
    {
        var row = new DynamicRow();
        row.AddMembersNoValue(ColumnNames);
        Rows.Add(row);
    }
    
    
    public bool ImportFile(string file)
    {
        throw new NotImplementedException("Data file import is not implemented!");
    }
    
    public bool ExportFile(string file)
    {
        throw new NotImplementedException("Data file export is not implemented!");
    }

    
}