using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public static Map instence;
    public GameObject roadPrefab;
    public Parcel[,] parcels;
    //public float[,] mapHeight;
    public AnimationCurve heightCurv;
    public AnimationCurve limitWaterCurv;
    public Mesh[,] mapMeshs;
    public GameObject[,] gfxsMap = new GameObject[20, 20];
    public GameObject gfxMapPrefab;
    public List<City> citys = new List<City>();

    private void Awake()
    {
        instence = this;
    }

    void Start()
    {
        parcels = new Parcel[1000, 1000];
        GenerateMap();
    }
    
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        float [,] mapNoise = NoiseGenerator.GenerNoise(1001 , 1001, 60, 3, 6, 0.1f, 10, heightCurv, limitWaterCurv);
        for (int y = 0; y < parcels.GetLength(1); y++)
        {
            for (int x = 0; x < parcels.GetLength(0); x++)
            {
                parcels[x, y] = new Parcel();
                parcels[x, y].corner = new int[4] { Mathf.FloorToInt(mapNoise[x, y]), Mathf.FloorToInt(mapNoise[x + 1, y]), Mathf.FloorToInt(mapNoise[x, y + 1]), Mathf.FloorToInt(mapNoise[x + 1, y + 1]) };
                parcels[x, y].pos = new Vector2Int(x, y);
            }
        }
        mapMeshs = MeshGenerator.MeshGenerat(parcels);
        for (int y = 0; y < mapMeshs.GetLength(1); y++)
        {
            for (int x = 0; x < mapMeshs.GetLength(0); x++)
            {
                gfxsMap[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform) ;
            }
        }

        while (citys.Count < 10)
        {
            CreatCity(Random.Range(100, 900), Random.Range(100, 900));
        }

        SetMesh();
    }
    public void UpdateChunk(int x, int y)
    {
        mapMeshs[x, y] = MeshGenerator.GenerateChunck(x, y, parcels);
        SetMesh();
    }

    public bool AddRoad(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y].construction == null)
        {
            Instantiate(roadPrefab, new Vector3(pos.x, parcels[pos.x, pos.y].corner[0], pos.y), Quaternion.identity, transform);
            parcels[pos.x, pos.y].construction = new Road();
            parcels[pos.x, pos.y].seeTerrain = false;
            UpdateChunk(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
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
    public bool CreatCity(int x, int y)
    {
        foreach(City _city in citys)
        {
            if (Vector3.Distance(new Vector3(x, 0f, y), _city.parent.position) < 100f)
            {
                return false;
            }
        }
        City city = new City();
        city.name = "City " + citys.Count;
        GameObject _go = new GameObject(city.name);
        city.parent = _go.transform;
        city.parent.position = new Vector3(x, 0f, y);
        city.parent.parent = GameObject.Find("Citys").transform;
        city.inhabitantsNumber = Random.Range(100, 1999);
        citys.Add(city);
        return true;
    }

    public void SetMesh()
    {
        for (int y = 0; y < mapMeshs.GetLength(1); y++)
        {
            for (int x = 0; x < mapMeshs.GetLength(0); x++)
            {
                gfxsMap[x, y].GetComponent<MeshFilter>().mesh = mapMeshs[x, y];
                gfxsMap[x, y].GetComponent<MeshCollider>().sharedMesh = mapMeshs[x, y];
            }
        }
        Debug.Log("Mesh Set");
    }
}

public class Parcel
{
    public Vector2Int pos;
    public int[] corner = new int[4];
    public object construction = null;
    public bool seeTerrain = true;

}