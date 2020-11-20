using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public  class Map
{
    public Parcel[,] parcels = new Parcel[1000, 1000];
    public List<City> citys = new List<City>();
    public List<Insdustrise> industrises =  new List<Insdustrise>();
    public bool[,] chunkNeedTextureUpdate = new bool[20, 20];
    public bool[,] chunkNeedMeshUpdate = new bool[20, 20];
    public async Task GenerateMap(AnimationCurve heightCurv, AnimationCurve limitWaterCurv)
    {
        parcels = new Parcel[1000, 1000];
        await Task.Delay(10);
        Debug.Log("Wait");
        citys = new List<City>();
        industrises = new List<Insdustrise>();
        float[,] mapNoise = NoiseGenerator.GenerNoise(1001, 1001, 60, 3, 6, 0.1f, 10, heightCurv, limitWaterCurv);
        for (int y = 0; y < parcels.GetLength(1); y++)
        {
            for (int x = 0; x < parcels.GetLength(0); x++)
            {
                parcels[x, y] = new Parcel();
                parcels[x, y].corner = new int[4] { Mathf.FloorToInt(mapNoise[x, y]), Mathf.FloorToInt(mapNoise[x + 1, y]), Mathf.FloorToInt(mapNoise[x, y + 1]), Mathf.FloorToInt(mapNoise[x + 1, y + 1]) };
                parcels[x, y].pos = new Vector2Int(x, y);
            }
        }
        await Task.Delay(10);
        Debug.Log("Wait");
        while (citys.Count < 20)
        {
            Debug.Log("city");
            await Task.Delay(10);
            Debug.Log("Wait");
            CreatCity(Random.Range(100, 900), Random.Range(100, 900));
            Debug.Log("Wait");
        }
        for (int i = 0; i < 50; i++)
        {
            CreatIndustrise(Random.Range(100, 900), Random.Range(100, 900));
            await Task.Delay(10);
            Debug.Log("Wait");
        }
        Debug.Log("finish");
    }

    public bool CreatCity(int x, int y)
    {
        Debug.Log("Creat City");
        foreach (City _city in citys)
        {
            if (Vector2Int.Distance(new Vector2Int(x, y), _city.pos) < 100f)
            {
                return false;
            }
        }
        Debug.Log("Creat City");
        //Transform _go = new GameObject().transform;
        //_go.position = new Vector3(x, 0f, y);
        //_go.parent = GameObject.Find("Citys").transform;
        City city = new City(new Vector2Int(x, y),this);
        city.name = "City " + citys.Count;
        city.inhabitantsNumber = Random.Range(100, 1999);
        citys.Add(city);
        Debug.Log("Creat City");
        return true;
    }

    public bool CreatIndustrise(int x, int y)
    {
        foreach (City _city in citys)
        {
            if (Vector2Int.Distance(new Vector2Int(x, y), _city.pos) < 30f)
            {
                return false;
            }
        }
        foreach (Insdustrise _industrise in industrises)
        {
            if (Vector2Int.Distance(new Vector2Int(x, y), _industrise.pos) < 10f)
            {
                return false;
            }
        }
        Transform _go = new GameObject().transform;
        _go.position = new Vector3(x, 0f, y);
        _go.parent = GameObject.Find("Instrises").transform;
        _go.name = "Industrise " + industrises.Count;
        Insdustrise insdustrise = new Insdustrise(new Vector2Int(x, y), this);
        industrises.Add(insdustrise);
        return true;
    }

    public bool AddRoad(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y].construction == null)
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
                    if (parcels[MapManager.parcelAroundCorner[_j].x + pos.x, MapManager.parcelAroundCorner[_j].y + pos.y].construction != null && parcels[MapManager.parcelAroundCorner[_j].x + pos.x, MapManager.parcelAroundCorner[_j].y + pos.y].construction.GetType() == typeof(Road))
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
            parcels[pos.x, pos.y].construction = new Road();
            for (int i = 0; i < MapManager.parcelAround.Length; i++)
            {
                if (parcels[MapManager.parcelAroundCorner[i].x + pos.x, MapManager.parcelAroundCorner[i].y + pos.y].construction != null && parcels[MapManager.parcelAroundCorner[i].x + pos.x, MapManager.parcelAroundCorner[i].y + pos.y].construction.GetType() == typeof(Road))
                {
                    ((Road)parcels[pos.x, pos.y].construction).direction[i] = true;
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
        if (parcels[pos.x, pos.y].construction == null)
        {

            //GameObject _go = Instantiate(buildingPrefab, new Vector3(pos.x, parcels[pos.x, pos.y].corner[0], pos.y), Quaternion.identity, transform);
            //_go.transform.GetChild(0).localPosition = new Vector3(0.5f, height / 2, 0.5f);
            //_go.transform.GetChild(0).localScale = new Vector3(1f, height, 1f);
            //_go.transform.parent = parent;
            //_go.transform.Find("GFX").GetComponent<Renderer>().material.color = color;
            parcels[pos.x, pos.y].construction = new Building();
            parcels[pos.x, pos.y].seeTerrain = false;
            UpdateChunkMesh(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            return true;

        }
        return false;
    }

    public bool AddDepot(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y].construction == null)
        {
            parcels[pos.x, pos.y].construction = new Depot(pos);
            UpdateChunkTexture(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            return true;
        }
        return false;
    }

    public void UpdateChunkTexture(int x, int y)
    {
        chunkNeedTextureUpdate[x, y] = true;
    }
    public void UpdateChunkMesh(int x, int y)
    {
        chunkNeedMeshUpdate[x, y] = true;
    }
}
