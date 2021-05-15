using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Object = UnityEngine.Object;

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

    public async Task SaveGame()
    {
        Debug.Log("Saving Started");
        await SaveMap();
        SaveIndustrise();
        SaveGroups();
        SaveStatistique();
        await AsyncTask.MonitorTask(SaveVehicle());
    }

    private async Task SaveMap()
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
                    Parcel parcel = map.GetParcel(new Vector2Int(x, y));
                    try
                    {
                        parcelsJsonList.Add(GetJson(parcel));
                    }
                    catch
                    {
                        parcel.DebugParcel();
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    mapForm[(y * map.Size.x + x) * 4 + i] = map.GetParcel<Parcel>(new Vector2Int(x, y)).corner[i];
                }
            }
        }
        await Task.Delay(1);
        timer.DebugTime("Creat Forme and JSON");
        await Task.Delay(1);
        //Debug.LogError("Finish");
        //return;
        string[] parcelsJsonArray = new string[parcelsJsonList.Count];
        for (int i = 0; i < parcelsJsonList.Count; i++)
        {
            parcelsJsonArray[i] = parcelsJsonList[i];
            //await AsyncTask.DelayIf(i, 100, 1);
            //await AsyncTask.DelayIfNeed(1);
        }
        await Task.Delay(1);
        timer.DebugTime("List => Array ");
        FIleSys.SaveFile(Path + "/mapForme.bin", mapForm);
        FIleSys.SaveFile(Path + "/mapConstruction.bin", parcelsJsonArray);
    }

    private async Task SaveVehicle()
    {
        string[] vehiclesJson = new string[vehicles.Count];
        for (int i = 0; i < vehicles.Count; i++)
        {
            vehiclesJson[i] = GetJson(VehicleDateStruct.GetStruct(vehicles[i]));
        }
        await Task.Delay(1);
        FIleSys.SaveFile(Path + "/vehicles.bin", vehiclesJson);
    }

    private void SaveStatistique()
    {
        Dictionary<string, object> statistiques = new Dictionary<string, object>();
        statistiques.Add("money", GameManager.Money);
        FIleSys.SaveFile(Path + "/statistique.bin", GetJson(statistiques));
	}

    private void SaveGroups()
    {
        string[] groupsJson = new string[Group.groups.Count];
        for (int i = 0; i < groupsJson.Length; i++)
        {
            groupsJson[i] = GetJson(Group.groups[i]);
        }
        FIleSys.SaveFile(Path + "/groups.bin", groupsJson);
    }

    private void SaveIndustrise()
    {
        string[] industriseJson = new string[map.industrises.Count];
        for (int i = 0; i < industriseJson.Length; i++)
        {
            Industrise curIndustrise = map.industrises[i];
            Dictionary<string, object> industriseDic = new Dictionary<string, object>();
            industriseDic.Add("pos", curIndustrise.MasterPos);
            industriseDic.Add("materialPoduction", curIndustrise.materialProductionRatio);
            industriseDic.Add("industriseData", Array.IndexOf(FIleSys.GetAllInstances<IndustriseData>(), curIndustrise.industriseData));
            for (int j = 0; j < 1; j++)
            {
                Dictionary<int, int> material = new Dictionary<int, int>();
                Dictionary<MaterialData, int> source = j == 0 ? curIndustrise.materialsInpute : curIndustrise.materialsOutpute;
                MaterialData[] materialList = FIleSys.GetAllInstances<MaterialData>();
                foreach (KeyValuePair<MaterialData, int> curMaterial in source)
                {
                    material.Add(Array.IndexOf(materialList, curMaterial.Key), curMaterial.Value);
                }
                industriseDic.Add(j == 0 ? "inpute" : "outpute", material);
            }
            industriseJson[i] = GetJson(industriseDic);
        }
        FIleSys.SaveFile(Path + "/industrise.bin", industriseJson);
    }

    

    public async Task LoadGame()
    {
        await LoadeMap();
        LoadGroups();
        Debug.Log("Finish");
    }
    public void LoadGameSeconde()
    {
        LoadStatistique();
        LoadIndustrise();
        LoadVehicles();
    }
    private async Task LoadeMap()
    {
        Timer timer = new Timer();
        //map = new Map();
        int[] mapForme = FIleSys.OpenFile<int[]>(Path + "/mapForme.bin");
        string[] parcelsJson = FIleSys.OpenFile<string[]>(Path + "/mapConstruction.bin");
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
            map.importParcels.Add(curParcel);
        }
        timer.DebugTime("Load Construction");
    }

    private void LoadVehicles()
    {
        string[] vehiclesJson = FIleSys.OpenFile<string[]>(Path + "/vehicles.bin");
        foreach(string curVehicleJson in vehiclesJson)
        {
            GameObject _go = Object.Instantiate(Resources.Load("Vehicle") as GameObject);
            GetObject<VehicleDateStruct>(curVehicleJson).SetVehiclePorpertise(_go.GetComponent<VehicleContoler>());
        }
    }

    private void LoadStatistique()
	{
        Dictionary<string, object> statistiques = GetObject<Dictionary<string, object>>(FIleSys.OpenFile<string>(Path + "/statistique.bin"));
        GameManager.Money = (long)statistiques["money"];
	}

    private void LoadGroups()
    {
        string[] groupsJson = FIleSys.OpenFile<string[]>(Path + "/groups.bin");
        Group.groups.Clear();
        foreach(string curGroupsJson in groupsJson)
        {
            Group.groups.Add(GetObject<Group>(curGroupsJson));
        }
    }

    private void LoadIndustrise()
    {
        MaterialData[] materialList = FIleSys.GetAllInstances<MaterialData>();
        string[] industriseJson = FIleSys.OpenFile<string[]>(Path + "/industrise.bin");
        for (int i = 0; i < industriseJson.Length; i++)
        {
            Dictionary<string, object> industriseValue = GetObject<Dictionary<string, object>>(industriseJson[i]);

            Industrise curIndustrise = new Industrise((Vector2Int)industriseValue["pos"], map, FIleSys.GetAllInstances<IndustriseData>()[(Int64)(industriseValue["industriseData"])]);
            curIndustrise.materialProductionRatio = (float)((double)industriseValue["materialPoduction"]);

            if (industriseValue.ContainsKey("inpute"))
            {
                foreach (KeyValuePair<int, int> curMaterial in (Dictionary<int, int>)industriseValue["inpute"])
                {
                    curIndustrise.materialsInpute[materialList[curMaterial.Key]] = curMaterial.Value;
                }
            }

            if (industriseValue.ContainsKey("outpute"))
            {
                foreach (KeyValuePair<int, int> curMaterial in (Dictionary<int, int>)industriseValue["outpute"])
                {
                    curIndustrise.materialsOutpute[materialList[curMaterial.Key]] = curMaterial.Value;
                }
            }

            map.industrises.Add(curIndustrise);
        }
    }

    public void ApplySave()
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

    private struct VehicleDateStruct
    {
        public int vehicleData;
        public float damage;
        public Route myRoute;
        public int RoutePointGo;
        public Vector2Int pos;
        public int myGroup;
        public int vehicleState;
        
        public static VehicleDateStruct GetStruct(VehicleContoler vehicle)
        {
            return new VehicleDateStruct
            {
                vehicleData = Array.IndexOf(FIleSys.GetAllInstances<VehicleData>(), vehicle.vehicleData),
                damage = vehicle.damage,
                myRoute = vehicle.MyRoute,
                RoutePointGo = vehicle.RoutePointGo,
                pos = vehicle.VehiclePos,
                myGroup = vehicle.MyGroup != null ? Group.groups.IndexOf(vehicle.MyGroup) : -1,
            };

        }
        public void SetVehiclePorpertise(VehicleContoler vehicle)
        {
            vehicle.vehicleData = FIleSys.GetAllInstances<VehicleData>()[vehicleData];
            vehicle.damage = damage;
            vehicle.MyRoute = myRoute;
            vehicle.RoutePointGo = RoutePointGo;
            vehicle.VehiclePos = pos;
            vehicle.MyGroup = Group.groups[myGroup];
        }
    }
}
