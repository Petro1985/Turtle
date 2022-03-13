using System.Text;
using ConsoleApp3.Maps;

namespace ConsoleApp3.Repositories;

public class InFileMapRepository : IMapRepository
{
    private string[] GetFileNamesList()
    {
        return Directory.GetFiles(".", "Map_*");
    }
    private string MapToString(Map map) {
        var str = new StringBuilder();

        str.AppendLine(map.Id.ToString());
        str.AppendLine(map.Name);
        var obj = map.GetAllObjects();
        
        foreach (var i in obj) {
            str.Append(i.Name);
            str.Append(';');
            str.Append(i.X);
            str.Append(';');
            str.Append(i.Y);
            str.AppendLine();
        }

        return str.ToString();
    }
    
    private Map MapFromString(string input) {
        
        var str = input.Trim().Split(Environment.NewLine);

        var map = new Map
        {
            Name = str[1],
        };

        typeof(Map).GetProperty(nameof(Map.Id))!.SetValue(map, int.Parse(str[0]));

        for (var i = 2; i < str.Length; i++) {
            MapObject mapObj;
            var tmpStr = str[i].Split(';');
            mapObj = tmpStr[0] switch {
                "Wall" => new Wall(int.Parse(tmpStr[1]), int.Parse(tmpStr[2])),
                _ => throw new Exception("Something went wrong! Hands are up!")
            };

            map.Add(mapObj);

        }

        return map;
    }
    
    public int Add(Map map)
    {
        var maxId = GetFileNamesList()
            .Select(item =>
            {
                int.TryParse(item.Split("_").ElementAtOrDefault(1), out var i);
                return i;
            }).DefaultIfEmpty(0).Max();
        
        var id = maxId + 1;
        var fileName = CreateFileName(id);

        typeof(Map).GetProperty(nameof(Map.Id))!.SetValue(map, id);
        var str = MapToString(map);
        
        File.WriteAllText(fileName, str);
        return id;
    }
    
    private string CreateFileName(int id) => $"Map_{id}";
    //private  Storage 
    public Map? Get(int id)
    {
        var fileName = CreateFileName(id);
        if (!File.Exists(fileName)) return null;
        
        var str = File.ReadAllText(fileName);
        
        return MapFromString(str);
    }

    public List<Map> GetAll()
    {
        return GetFileNamesList()
            .Select(file => MapFromString(File.ReadAllText(file)))
            .ToList();
    }
}