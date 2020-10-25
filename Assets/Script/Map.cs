using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    public static Map instence;
    public GameObject roadPrefab;
    public GameObject buildingPrefab;
    public Parcel[,] parcels;
    //public float[,] mapHeight;
    public AnimationCurve heightCurv;
    public AnimationCurve limitWaterCurv;
    public Mesh[,] mapMeshs;
    public Texture2D[,] mapTexture;
    public GameObject[,] gfxsMap = new GameObject[20, 20];
    public GameObject gfxMapPrefab;
    public List<City> citys = new List<City>();
    public List<Insdustrise> industrises = new List<Insdustrise>();

    private bool[,] chunkNeedUpdate = new bool[20, 20];

    public static readonly Vector2Int[] parcelAround = 
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    public static readonly Vector2Int[] parcelAroundCorner =
   {
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1)
    };

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
    private void FixedUpdate()
    {
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (chunkNeedUpdate[x, y])
                {
                    mapMeshs[x, y] = MeshGenerator.GenerateChunck(x, y, parcels);
                    mapTexture[x, y] = TerxtureGennerator.GenerateTextureChunck(x, y, parcels);
                    SetMesh();
                    chunkNeedUpdate[x, y] = false;
                }
            }
        }
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
        mapTexture = TerxtureGennerator.GeneratTexture(parcels);
        for (int y = 0; y < mapMeshs.GetLength(1); y++)
        {
            for (int x = 0; x < mapMeshs.GetLength(0); x++)
            {
                gfxsMap[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform);
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
        SetMesh();
    }
    public void UpdateChunk(int x, int y)
    {
        chunkNeedUpdate[x, y] = true;
    }

    public bool AddRoad(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y].construction == null)
        {
            for (int j = 0; j < parcelAroundCorner.Length; j++)
            {
                if (parcelAroundCorner[j].x == 0 || parcelAroundCorner[j].y == 0)
                {
                    continue;
                }
                int _countRoad = 0;
                for (int h = -1; h <= 1; h++)
                {
                    int _j = (j + h) % parcelAroundCorner.Length;
                    if (_j == -1)
                    {
                        _j = parcelAroundCorner.Length - 1;
                    }
                    if (parcels[parcelAroundCorner[_j].x + pos.x, parcelAroundCorner[_j].y + pos.y].construction != null && parcels[parcelAroundCorner[_j].x + pos.x, parcelAroundCorner[_j].y + pos.y].construction.GetType() == typeof(Road))
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
            for (int i = 0; i < parcelAround.Length; i++)
            {
                if (parcels[parcelAroundCorner[i].x + pos.x, parcelAroundCorner[i].y + pos.y].construction != null && parcels[parcelAroundCorner[i].x + pos.x, parcelAroundCorner[i].y + pos.y].construction.GetType() == typeof(Road))
                {
                    ((Road)parcels[pos.x, pos.y].construction).direction[i] = true;
                }
            }
            //parcels[pos.x, pos.y].seeTerrain = false;
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
    public bool AddBuilding(Vector2Int pos, float height, Transform parent, Color color)
    {
        if (parcels[pos.x, pos.y].construction == null)
        {
            
            GameObject _go = Instantiate(buildingPrefab, new Vector3(pos.x, parcels[pos.x, pos.y].corner[0], pos.y), Quaternion.identity, transform);
            _go.transform.GetChild(0).localPosition = new Vector3(0.5f, height / 2, 0.5f);
            _go.transform.GetChild(0).localScale = new Vector3(1f, height, 1f);
            _go.transform.parent = parent;
            _go.transform.Find("GFX").GetComponent<Renderer>().material.color = color;
            parcels[pos.x, pos.y].construction = new Building();
            parcels[pos.x, pos.y].seeTerrain = false;
            UpdateChunk(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
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
        Transform _go = new GameObject().transform;
        _go.position = new Vector3(x, 0f, y);
        _go.parent = GameObject.Find("Citys").transform;
        City city = new City(_go);
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
        Insdustrise insdustrise = new Insdustrise(_go);
        industrises.Add(insdustrise);
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
                gfxsMap[x, y].GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                gfxsMap[x, y].GetComponent<Renderer>().sharedMaterial.mainTexture = mapTexture[x, y];
            }
        }
        //Debug.Log("Mesh Set");
    }
}

public class Parcel
{
    public Vector2Int pos;
    public int[] corner = new int[4];
    public object construction = null;
    public bool seeTerrain = true;

}