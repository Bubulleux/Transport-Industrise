using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public  class MapData
{
    public Parcel[,] parcels = new Parcel[1000, 1000];
    public List<City> citys = new List<City>();
    public List<Insdustrise> industrises =  new List<Insdustrise>();
    public Task GenerateMap(AnimationCurve heightCurv, AnimationCurve limitWaterCurv)
    {
        parcels = new Parcel[1000, 1000];
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

        while (citys.Count < 20)
        {
            CreatCity(Random.Range(100, 900), Random.Range(100, 900));
        }
        for (int i = 0; i < 50; i++)
        {
            CreatIndustrise(Random.Range(100, 900), Random.Range(100, 900));
        }
        return Task.CompletedTask;
    }

    public bool CreatCity(int x, int y)
    {
        foreach (City _city in citys)
        {
            if (Vector3.Distance(new Vector3(x, 0f, y), _city.parent.position) < 100f)
            {
                return false;
            }
        }
        Transform _go = new GameObject().transform;
        _go.position = new Vector3(x, 0f, y);
        _go.parent = GameObject.Find("Citys").transform;
        City city = new City(_go, this);
        city.name = "City " + citys.Count;
        city.inhabitantsNumber = Random.Range(100, 1999);
        citys.Add(city);
        return true;
    }

    public bool CreatIndustrise(int x, int y)
    {
        foreach (City _city in citys)
        {
            if (Vector3.Distance(new Vector3(x, 0f, y), _city.parent.position) < 30f)
            {
                return false;
            }
        }
        foreach (Insdustrise _industrise in industrises)
        {
            if (Vector3.Distance(new Vector3(x, 0f, y), _industrise.parent.position) < 10f)
            {
                return false;
            }
        }
        Transform _go = new GameObject().transform;
        _go.position = new Vector3(x, 0f, y);
        _go.parent = GameObject.Find("Instrises").transform;
        _go.name = "Industrise " + industrises.Count;
        Insdustrise insdustrise = new Insdustrise(_go, this);
        industrises.Add(insdustrise);
        return true;
    }

    public bool AddRoad(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y].construction == null)
        {
            for (int j = 0; j < Map.parcelAroundCorner.Length; j++)
            {
                if (Map.parcelAroundCorner[j].x == 0 || Map.parcelAroundCorner[j].y == 0)
                {
                    continue;
                }
                int _countRoad = 0;
                for (int h = -1; h <= 1; h++)
                {
                    int _j = (j + h) % Map.parcelAroundCorner.Length;
                    if (_j == -1)
                    {
                        _j = Map.parcelAroundCorner.Length - 1;
                    }
                    if (parcels[Map.parcelAroundCorner[_j].x + pos.x, Map.parcelAroundCorner[_j].y + pos.y].construction != null && parcels[Map.parcelAroundCorner[_j].x + pos.x, Map.parcelAroundCorner[_j].y + pos.y].construction.GetType() == typeof(Road))
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
            for (int i = 0; i < Map.parcelAround.Length; i++)
            {
                if (parcels[Map.parcelAroundCorner[i].x + pos.x, Map.parcelAroundCorner[i].y + pos.y].construction != null && parcels[Map.parcelAroundCorner[i].x + pos.x, Map.parcelAroundCorner[i].y + pos.y].construction.GetType() == typeof(Road))
                {
                    ((Road)parcels[pos.x, pos.y].construction).direction[i] = true;
                }
            }
            //parcels[pos.x, pos.y].seeTerrain = false;
            //UpdateChunk(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
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

    public bool AddBuilding(Vector2Int pos, float height, Transform parent, Color color)
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
            //UpdateChunk(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            return true;

        }
        return false;
    }
}
