using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class MapManager : MonoBehaviour
{
    public static MapManager instence;

    public GameObject roadPrefab;
    public GameObject buildingPrefab;
    //public float[,] mapHeight;

    public Mesh[,] mapMeshs;
    public Texture2D[,] mapTexture;
    public GameObject[,] gfxsMap = new GameObject[20, 20];
    public GameObject gfxMapPrefab;


    public static Map map;

    public delegate void MapUpdateEventDelegate();
    public event MapUpdateEventDelegate MapUpdateEvent;

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
        if (instence == null)
        {
            enabled = false;
        }
        instence = this;
    }

    public void CreateChunck(Mesh[,] meshs, Texture2D[,] texrures)
    {
        for (int y = 0; y < gfxsMap.GetLength(0); y++)
        {
            for (int x = 0; x < gfxsMap.GetLength(0); x++)
            {
                gfxsMap[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform);
                gfxsMap[x, y].GetComponent<MeshFilter>().mesh = meshs[x, y];
                gfxsMap[x, y].GetComponent<MeshCollider>().sharedMesh = meshs[x, y];
                gfxsMap[x, y].GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard"))
                {
                    mainTexture = texrures[x, y]
                };
                map.chunkNeedMeshUpdate[x, y] = false;
                map.chunkNeedTextureUpdate[x, y] = false;
            }
        }
    }

    void FixedUpdate()
    {
        UpdateMap();
        if (GameLoader.load == GameLoader.LoadStatus.Done)
        {
            UpdateMap(); 
            foreach (Industrise curIndustrise in map.industrises)
            {
                curIndustrise.Update(Time.fixedDeltaTime);
            }
        }
    }

    public void UpdateEveryChunck()
    {
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                map.chunkNeedMeshUpdate[x, y] = true;
                map.chunkNeedTextureUpdate[x, y] = true;
            }
        }
    }

    public void UpdateMap()
    {
        bool mapHasBeenUpdate = false;
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {

                if (map.chunkNeedTextureUpdate[x, y] || map.chunkNeedMeshUpdate[x, y])
                {
                    if (map.chunkNeedMeshUpdate[x, y])
                    {
                        MeshGenerator.AsyncGenerateChunk(new Vector2Int(x,y), map, gfxsMap[x, y]);
                        map.chunkNeedMeshUpdate[x, y] = false;
                    }
                    if (map.chunkNeedTextureUpdate[x, y])
                    {
                        TextureGenerator.AsyncGenerateTextureChunk(new Vector2Int(x,y) , map, gfxsMap[x, y]);
                        map.chunkNeedTextureUpdate[x, y] = false;
                    }
                    mapHasBeenUpdate = true;
                }
            }
        }
        if (mapHasBeenUpdate)
        {
            MapUpdateEvent?.Invoke();
        }
    }

}