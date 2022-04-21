using System.Collections.ObjectModel;
using MockApi.Data;

namespace MockApi.Services;

public class DataService
{
    public List<string> ColumnNames = new();

    public ObservableCollection<DynamicRow> Rows { get; } = new ();
    
    public DataService()
    {
        ColumnNames = new List<string> { "Column1", "Column2", "Column3"};
    }
    

    
    /// <summary>
    /// Updates all the columns in the row with the new column values
    /// </summary>
    public void UpdateRowColumns()
    {
        foreach (var row in Rows)
        {
            row.RenameColumns(ColumnNames);
        }
    }

    /// <summary>
    /// Finds all rows that match the column/value pair
    /// </summary>
    /// <param name="column">name of column to check</param>
    /// <param name="withValue">value to check column against</param>
    /// <returns>an IEnumerable containing all rows that match</returns>
    public IEnumerable<DynamicRow> FindAll(string column, string withValue)
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
        ColumnNames.Add(name);
        foreach (var row in Rows)
        {
            row.AddColumn(name);
        }
        return true;
    }
    
    public void AddRow()
    {
        var row = new DynamicRow();
        row.AddColumnsNoValues(ColumnNames);

        // assign each row an ID value
        if (Rows.Count > 0)
        {
            // get last rows id and increment it by 1
            row.RowID = Rows.Last().RowID;
            row.RowID++;
        }
        else
            row.RowID = 0;
        
        Rows.Add(row);
    }

    public void DeleteRows(IEnumerable<DynamicRow> rowsToRemove)
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