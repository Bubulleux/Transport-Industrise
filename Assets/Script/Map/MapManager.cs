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

    public AnimationCurve heightCurv;
    public AnimationCurve limitWaterCurv;

    public Mesh[,] mapMeshs;
    public Texture2D[,] mapTexture;
    public GameObject[,] gfxsMap = new GameObject[20, 20];
    public GameObject gfxMapPrefab;


    public static Map map;
    public static int startTimeOfGame;

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
        if (startTimeOfGame == 0)
        {
            startTimeOfGame = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60;
            Debug.Log("Awake");
        }
        instence = this;
    }

    void Start()
    {
        if (!MapLoader.load)
        {
            return;
        }
        Debug.Log(map.citys.Count);
        int startTime = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60;
        //mapMeshs = MeshGenerator.MeshGenerat(map.parcels);
        //mapTexture = TerxtureGennerator.GeneratTexture(map.parcels);
        Debug.Log($"time creat mesh and texture: {System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 - startTime}");
        startTime = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60;
        for (int y = 0; y < gfxsMap.GetLength(0); y++)
        {
            for (int x = 0; x < gfxsMap.GetLength(0); x++)
            {
                gfxsMap[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform);
                map.chunkNeedMeshUpdate[x, y] = true;
                map.chunkNeedTextureUpdate[x, y] = true;
            }
        }
        //SetMesh();
        Debug.Log($"time creat chunck: {System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 - startTime}");
        Debug.Log($"time: {System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 - startTimeOfGame}");
    }

    private void FixedUpdate()
    {
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (map.chunkNeedTextureUpdate[x, y] || map.chunkNeedMeshUpdate[x, y])
                {
                    if (map.chunkNeedMeshUpdate[x, y])
                    {
                        MeshGenerator.AsyncGenerateChunck(x, y, map.parcels, gfxsMap[x, y]);
                        map.chunkNeedMeshUpdate[x, y] = false;
                    }
                    if (map.chunkNeedTextureUpdate[x, y])
                    {
                        TerxtureGennerator.AsyncGenerateTextureChunck(x, y, map.parcels, gfxsMap[x, y]);
                        map.chunkNeedTextureUpdate[x, y] = false;
                    }
                }
            }
        }
    }
}