using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public  class Map
{
    [JsonProperty]
    public Parcel[,] parcels = new Parcel[1000, 1000];
    public Vector2Int Size { get => new Vector2Int(parcels.GetLength(0), parcels.GetLength(1)); }
    [JsonProperty]
    public List<City> citys = new List<City>();
    [JsonProperty]
    public List<Industrise> industrises =  new List<Industrise>();


    public bool[,] chunkNeedTextureUpdate = new bool[20, 20];
    public bool[,] chunkNeedMeshUpdate = new bool[20, 20];
    public List<Parcel> importParcels = new List<Parcel>();


    public async Task GenerateMap(MapSettingData mapSetting)
    {
        parcels = new Parcel[1000, 1000];
        await Task.Delay(10);
        citys = new List<City>();
        industrises = new List<Industrise>();
        float[,] mapNoise = NoiseGenerator.GenerNoise(1001, 1001, 60, 3, 6, 0.1f, Random.Range(1, 10000), mapSetting.heightCurv, mapSetting.limitWaterCurv);
        for (int y = 0; y < parcels.GetLength(1); y++)
        {
            for (int x = 0; x < parcels.GetLength(0); x++)
            {
                parcels[x, y] = new Parcel(new Vector2Int(x, y))
                {
                    corner = new int[4] { Mathf.FloorToInt(mapNoise[x, y]), Mathf.FloorToInt(mapNoise[x + 1, y]), Mathf.FloorToInt(mapNoise[x, y + 1]), Mathf.FloorToInt(mapNoise[x + 1, y + 1]) }
                };
            }
        }
        await Task.Delay(10);
        while (citys.Count < 20)
        {
            await Task.Delay(10);;
            CreatCity(Random.Range(100, 900), Random.Range(100, 900));
        }
        for (int i = 0; i < 50; i++)
        {
            CreatIndustrise(new Vector2Int(Random.Range(100, 900), Random.Range(100, 900)));
            await Task.Delay(10);
        }
    }

    public T GetParcel<T>(Vector2Int pos) where T : Parcel
    {
        return parcels[pos.x, pos.y] as T;
    }
    public Parcel GetParcel(Vector2Int pos) 
    {
        return parcels[pos.x, pos.y];
    }

    public System.Type GetparcelType(Vector2Int pos)
    {
        return parcels[pos.x, pos.y].GetType();
    }

    public bool CreatCity(int x, int y)
    {
        foreach (City _city in citys)
        {
            if (Vector2Int.Distance(new Vector2Int(x, y), _city.MasterPos) < 100f)
            {
                return false;
            }
        }
        //Transform _go = new GameObject().transform;
        //_go.position = new Vector3(x, 0f, y);
        //_go.parent = GameObject.Find("Citys").transform;
        City city = new City(new Vector2Int(x, y), this)
        {
            name = "City " + citys.Count,
            inhabitantsNumber = Random.Range(100, 1999)
        };
        citys.Add(city);
        return true;
    }

    public Industrise CreatIndustrise(Vector2Int pos)
    {
        foreach (City _city in citys)
        {
            if (Vector2Int.Distance(pos, _city.MasterPos) < 50f)
            {
                return null;
            }
        }
        foreach (Industrise _industrise in industrises)
        {
            if (Vector2Int.Distance(pos, _industrise.MasterPos) < 10f)
            {
                return null;
            }
        }
        //Transform _go = new GameObject().transform;
        //_go.position = new Vector3(x, 0f, y);
        //_go.parent = GameObject.Find("Instrises").transform;
        //_go.name = "Industrise " + industrises.Count;
        Industrise Industrise = new Industrise(pos, this);
        industrises.Add(Industrise);
        return Industrise;
    }

    public bool AddRoad(Vector2Int pos)
    {
        if (pos.x < 50 || pos.x >= 950 || pos.y < 50 || pos.y >= 950)
        {
            Debug.Log("Return");
            return false;
        }
        if (parcels[pos.x, pos.y].GetType() == typeof(Parcel))
        {
            for (int j = 0; j < MapManager.parcelAroundCorner.Length; j++)
            {
                if (MapManager.parcelAroundCorner[j].x == 0 || MapManager.parcelAroundCorner[j].y == 0)
                {
                    continue;
                }
                int _countRoad = 0;
                for (int h = -1; h <= 1; h++)
                {
                    int _j = (j + h) % MapManager.parcelAroundCorner.Length;
                    if (_j == -1)
                    {
                        _j = MapManager.parcelAroundCorner.Length - 1;
                    }
                    if (parcels[MapManager.parcelAroundCorner[_j].x + pos.x, MapManager.parcelAroundCorner[_j].y + pos.y].GetType() != typeof(Parcel) && parcels[MapManager.parcelAroundCorner[_j].x + pos.x, MapManager.parcelAroundCorner[_j].y + pos.y].GetType() == typeof(Road))
                    {
                        _countRoad++;
                    }
                    if (_countRoad == 3)
                    {
                        return false;
                    }
                }
            }
            //Instantiate(roadPrefab, new Vector3(pos.x, parcels[pos.x, pos.y].corner[0], pos.y), Quaternion.identity, transform);
            parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], new Road());
            for (int i = 0; i < MapManager.parcelAround.Length; i++)
            {
                if (parcels[MapManager.parcelAroundCorner[i].x + pos.x, MapManager.parcelAroundCorner[i].y + pos.y].GetType() != typeof(Parcel) && parcels[MapManager.parcelAroundCorner[i].x + pos.x, MapManager.parcelAroundCorner[i].y + pos.y].GetType() == typeof(Road))
                {
                    GetParcel<Road>(pos).direction[i] = true;
                }
            }
            //parcels[pos.x, pos.y].seeTerrain = false;
            UpdateChunkTexture(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            //if (mapHeight[pos.x, pos.y] != mapHeight[pos.x, pos.y + 1] || mapHeight[pos.x + 1 , pos.y] != mapHeight[pos.x + 1 , pos.y + 1] || mapHeight[pos.x, pos.y] != mapHeight[pos.x + 1, pos.y])
            //{
            //    mapHeight[pos.x, pos.y + 1] = mapHeight[pos.x, pos.y];
            //    mapHeight[pos.x + 1, pos.y + 1] = mapHeight[pos.x, pos.y];
            //    mapHeight[pos.x + 1, pos.y] = mapHeight[pos.x, pos.y];
            //}
            return true;

        }
        return false;
    }

    public bool AddBuilding(Vector2Int pos, float height, Color color)
    { 
        if (parcels[pos.x, pos.y].GetType() == typeof(Parcel))
        {
            Building building = new Building
            {
                color = color,
                height = height
            };
            parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], building);
            //parcels[pos.x, pos.y].seeTerrain = false;
            UpdateChunkMesh(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            return true;

        }
        return false;
    }

    public bool AddConstruction(Vector2Int pos, Parcel construction)
    {
        if (GetparcelType(pos) == typeof(Parcel))
        {
            parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], construction);
            UpdateChunkTexture(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            importParcels.Add(construction);
            return true;
        }
        return false;
    }

    public bool Destroy(Vector2Int pos)
    {
        if (GetparcelType(pos) == typeof(Parcel) || (GetparcelType(pos) == typeof(Road) && VehicleManager.GetVehicleByPos(GetParcel<Parcel>(pos)).Count != 0))
        {
            return false;
        }
        parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], new Parcel());
        UpdateChunkTexture(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
        importParcels.Remove(parcels[pos.x, pos.y]);
        return true;
    }

    public void Color(Vector2Int pos, Color color)
    {
        Building building = new Building
        {
            color = color,
            height = 0
        };
        parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], building);
        //parcels[pos.x, pos.y].seeTerrain = false;
        UpdateChunkTexture(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
    }

    public void UpdateChunkTexture(int x, int y)
    {
        chunkNeedTextureUpdate[x, y] = true;
    }
    public void UpdateChunkMesh(int x, int y)
    {
        chunkNeedMeshUpdate[x, y] = true;
    }

    public T[] GetImpotantParcel<T>() where T : Parcel
    {
        List<T> parcelsList = new List<T>();
        foreach(Parcel curParcel in importParcels)
        {
            if (curParcel.GetType() == typeof(T))
            {
                parcelsList.Add(curParcel as T);
            }
        }
        T[] parcelArray = new T[parcelsList.Count];
        for (int i = 0; i < parcelsList.Count; i++)
        {
            parcelArray[i] = parcelsList[i];
        }
        return parcelArray;

    }
}
