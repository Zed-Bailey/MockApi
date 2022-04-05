using System.Dynamic;
using MudBlazor;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MockApi.Data;

namespace MockApi.Services;

public class DataService
{
    public List<string> ColumnNames = new();

    public ObservableCollection<DynamicRow_rewrite> Rows { get; } = new ();
    
    public DataService()
    {
        ColumnNames = new List<string> { "Column1", "Column2", "Column3"};
    
    }
    

    public void UpdateRows()
    {
        foreach (var row in Rows)
        {
            row.RenameColumns(ColumnNames);
        }
    }

    public IEnumerable<DynamicRow_rewrite> FindAll(string column, string withValue)
    {
        var list = Rows.ToList();
        // find all the rows where there is a matching column
        return list.FindAll(x => x.ColumnMatches(column, withValue));
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
        return true;
    }
    
    public void AddRow()
    {
        var row = new DynamicRow_rewrite();
        row.AddColumnsNoValues(ColumnNames);
        Rows.Add(row);
    }

    public void DeleteRows(IEnumerable<DynamicRow_rewrite> rowsToRemove)
    {
        foreach (var d in rowsToRemove)
        {
            Rows.Remove(d);
        }
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