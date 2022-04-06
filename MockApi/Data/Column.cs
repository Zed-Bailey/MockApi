namespace MockApi.Data;

public class Column
{
    public string Key { get; set; }
    public string? Value { get; set; }
    public bool Editable { get; set; }

    public Column(string name)
    {
        Key = name;
        Value = null;
        Editable = false;
        
    }
}