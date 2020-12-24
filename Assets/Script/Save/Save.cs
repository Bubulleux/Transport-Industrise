using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[JsonObject(MemberSerialization.OptOut)]
public class Save
{
    public Map map;
    public List<VehicleContoler> vehicles;
    public List<Group> group;

    [JsonIgnore]
    public static JsonSerializerSettings setting = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.All,
        //Formatting = Formatting.Indented
    };
    [JsonIgnore]
    public string path = "/Save";


    public Save()
    {
        map = MapManager.map;
        vehicles = VehicleManager.Vehicles;
        group = Group.groups;
    }

    public async Task SerializeAndSave()
    {
        await SerializeAndSaveMap();
        Debug.Log("Finish");
    }

    private async Task SerializeAndSaveMap()
    {
        TextWriter()

        return;
        string[] parcelsJson = new string[map.Size.x * map.Size.y];
        for (int y = 0; y < map.Size.y; y++)
        {
            for (int x = 0; x < map.Size.x; x++)
            {
                parcelsJson[y * map.Size.x + x] = GetJson(map.GetParcel(new Vector2Int(x, y)));
            }
            await AsyncTask.DelayIf(y, 10, 1);
        }
        FIleSys.SaveFile(path + "/map.dat", parcelsJson);
    }

    public async Task LoadAndDeserialize()
    {
        await LoadAndDeserializeMap();
        LoadSave();
        Debug.Log("Finish");

    }
    private async Task LoadAndDeserializeMap()
    {
        //map = new Map();
        string[] parcelsJson = FIleSys.OpenFile<string[]>(path + "/map.dat");
        for (int i = 0; i < parcelsJson.Length; i++)
        {
            Parcel curParcel = GetObject<Parcel>(parcelsJson[i]);
            map.parcels[curParcel.pos.x, curParcel.pos.y] = curParcel;
            await AsyncTask.DelayIf(i, 10000, 1);
        }
    }

    public void LoadSave()
    {
        MapManager.map = map;
        MapManager.instence.UpdateEveryChunck();
    }

    public static string GetJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, setting);
    }
    public static T GetObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, setting);
    }
}
