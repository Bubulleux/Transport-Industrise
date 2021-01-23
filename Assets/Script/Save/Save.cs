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
    public string name;
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
    public string Path { get { return "/Save/" + name; } }


    public Save()
    {
        name = GameManager.saveName;
        map = MapManager.map;
        vehicles = VehicleManager.Vehicles;
        group = Group.groups;
    }

    public Save(string saveName)
    {
        name = saveName;
    }

    public async Task SerializeAndSave()
    {
        Debug.Log("Saving Started");
        Task operation = SerializeAndSaveMap();
        await Task.Delay(1000);
        while(!operation.IsFaulted && !operation.IsCompleted)
        {
            await Task.Delay(100);
        }
        if (operation.IsFaulted)
        {
            Debug.LogException(operation.Exception);
        }
        Debug.Log($"Finish {operation.Status} {operation.IsCompleted}, {operation.IsFaulted}");
    }

    private async Task SerializeAndSaveMap()
    {
        Timer timer = new Timer();
        List<string> parcelsJsonList = new List<string>();
        int[] mapForm = new int[map.Size.x * map.Size.y * 4];
        for (int y = 0; y < map.Size.y; y++)
        {
            for (int x = 0; x < map.Size.x; x++)
            {
                if (map.GetparcelType(new Vector2Int(x, y)) != typeof(Parcel))
                {
                    parcelsJsonList.Add(GetJson(map.GetParcel(new Vector2Int(x, y))));
                }
                for (int i = 0; i < 4; i++)
                {
                    mapForm[(y * map.Size.x + x) * 4 + i] = map.GetParcel<Parcel>(new Vector2Int(x, y)).corner[i];
                }
                //await AsyncTask.DelayIfNeed(1);
            }
        }
        timer.DebugTime("Creat Forme and JSON");
        //Debug.LogError("Finish");
        //return;
        string[] parcelsJsonArray = new string[parcelsJsonList.Count];
        for (int i = 0; i < parcelsJsonList.Count; i++)
        {
            parcelsJsonArray[i] = parcelsJsonList[i];
            //await AsyncTask.DelayIf(i, 100, 1);
            //await AsyncTask.DelayIfNeed(1);
        }
        timer.DebugTime("List => Array ");
        FIleSys.SaveFile(Path + "/mapForme.dat", mapForm);
        FIleSys.SaveFile(Path + "/mapConstruction.dat", parcelsJsonArray);
    }

    public async Task LoadAndDeserialize()
    {
        Task operation = LoadAndDeserializeMap();
        while(!operation.IsCompleted && !operation.IsFaulted)
        {
            await Task.Delay(100);
        }
        if (operation.IsFaulted)
        {
            Debug.LogException(operation.Exception);
        }
        //LoadSave();
        Debug.Log("Finish");

    }
    private async Task LoadAndDeserializeMap()
    {
        Timer timer = new Timer();
        //map = new Map();
        int[] mapForme = FIleSys.OpenFile<int[]>(Path + "/mapForme.dat");
        string[] parcelsJson = FIleSys.OpenFile<string[]>(Path + "/mapConstruction.dat");
        timer.DebugTime("Load File");
        map = new Map(new Vector2Int(20, 20));
        await Task.Delay(1);
        for (int i = 0; i < mapForme.Length; i += 4)
        {
            int x = (i / 4) % map.Size.x;
            int y = (i  / 4 - x ) / map.Size.x;
            map.parcels[x, y] = new Parcel(new Vector2Int(x, y));
            for (int j = 0; j < 4; j++)
            {
                map.parcels[x, y].corner[j] = mapForme[i + j];
            }
            
        }
        timer.DebugTime("Load Forme");
        await Task.Delay(1);
        for (int i = 0; i < parcelsJson.Length; i++)
        {
            Parcel curParcel = GetObject<Parcel>(parcelsJson[i]);
            map.parcels[curParcel.pos.x, curParcel.pos.y] = curParcel;
        }
        timer.DebugTime("Load Construction");
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
