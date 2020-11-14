using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    public static Map instence;

    public GameObject roadPrefab;
    public GameObject buildingPrefab;
    //public float[,] mapHeight;

    public AnimationCurve heightCurv;
    public AnimationCurve limitWaterCurv;

    public Mesh[,] mapMeshs;
    public Texture2D[,] mapTexture;
    public GameObject[,] gfxsMap = new GameObject[20, 20];
    public GameObject gfxMapPrefab;

    private bool[,] chunkNeedUpdate = new bool[20, 20];

    public static MapData mapData;
    public Parcel[,] parcels { get => mapData.parcels; set => mapData.parcels = value; }
    public List<City> citys { get => mapData.citys; set => mapData.citys = value; }
    public List<Insdustrise> industrises { get => mapData.industrises; set => mapData.industrises = value; }

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
        Debug.Log(mapData.citys.Count);
        mapMeshs = MeshGenerator.MeshGenerat(parcels);
        mapTexture = TerxtureGennerator.GeneratTexture(parcels);
        for (int y = 0; y < mapMeshs.GetLength(1); y++)
        {
            for (int x = 0; x < mapMeshs.GetLength(0); x++)
            {
                gfxsMap[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform);
                UpdateChunk(x, y);
            }
        }
        SetMesh();
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

    public void UpdateChunk(int x, int y)
    {
        chunkNeedUpdate[x, y] = true;
    }

    

    public bool AddDepot(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y].construction == null)
        {
            parcels[pos.x, pos.y].construction = new Depot(pos);
            UpdateChunk(Mathf.FloorToInt(pos.x / 50), Mathf.FloorToInt(pos.y / 50));
            return true;
        }
        return false;
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