using System.Collections.ObjectModel;

namespace MockApi.Data;

public class DynamicRow_rewrite
{
    public List<Column> Columns = new();

    public int Count => Columns.Count;

    public bool AddColumn(string name)
    {
        // check if column exists, if so return false
        if (Columns.Any(x => x.Key == name)) return false;
        
        // add column
        Columns.Add(new Column(name));
        return true;
    }

    public void RenameColumns(IEnumerable<string> newNames)
    {
        // cast IEnumerable to string so i can use indexes on it
        var names = newNames.ToList();
        
        for (int i = 0; i < newNames.Count(); i++)
        {
            Columns[i].Key = names[i];
        }
        
    }

    public bool DeleteColumn(string name)
    {
        var colToDelete = Columns.First(x => x.Key == name);
        return Columns.Remove(colToDelete);
    }
    

    public void AddColumnsNoValues(IEnumerable<string> columnNames)
    {
        foreach (var name in columnNames)
        {
            Columns.Add(new Column(name));
        }
    }
}