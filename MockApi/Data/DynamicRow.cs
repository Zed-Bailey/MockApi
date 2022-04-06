using System.Collections.ObjectModel;

namespace MockApi.Data;

public class DynamicRow
{
    public List<Column> Columns = new();

    // public int Count => Columns.Count;
    public int RowID;

    public bool ColumnMatches(string column, string forValue)
    {
        var col = Columns.Find(x => x.Key == column);
        if (col != null)
        {
            var match = col.Value == forValue;
            Console.WriteLine("Found column match!");
            return match;
        }

        return false;
    }
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