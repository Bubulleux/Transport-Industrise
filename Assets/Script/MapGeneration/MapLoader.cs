using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapLoader : MonoBehaviour
{
    public static MapLoader instence;
    public Task operation;
    public static LoadStatus load = LoadStatus.NotStart;

    public string LoadingIndicator { set { GameObject.Find("OperationState").GetComponent<Text>().text = value; } }

    public int Width { get =>  MapManager.map.parcels.GetLength(0) / 50;  }
    public int Height { get =>  MapManager.map.parcels.GetLength(1) / 50;  }


    private void Awake()
    {
        instence = this;
    }

    public static void GenerateMap(string saveName)
    {
        instence.operation = instence.AsyncGeneratedMap(saveName);
    }

    public static void LoadSave(string saveName)
    {
        instence.operation = instence.AsyncLoadSave(saveName);
    }

    public async Task AsyncGeneratedMap(string saveName)
    {
        await AsyncLoadScene(1);

        LoadingIndicator = "Generate Map";
        Map _mapdata = new Map(new Vector2Int(20, 20));
        await _mapdata.GenerateMap(FIleSys.GetAllInstances<MapSettingData>()[0]);
        MapManager.map = _mapdata;
        GameManager.saveName = saveName;

        await LoadGame();

        new Save().SerializeAndSave();

        load = LoadStatus.Done;
    }

    public async Task AsyncLoadSave(string saveName)
    {
        await AsyncLoadScene(1);
        LoadingIndicator = "Load Save: " + saveName;
        Save save = new Save(saveName);
        await Task.Delay(1);
        await save.LoadAndDeserialize();

        MapManager.map = save.map;
        GameManager.saveName = save.name;

        await LoadGame();
        load = LoadStatus.Done;
    }

    public async Task LoadGame()
    {
        Mesh[,] meshs = await LoadEveryMesh();

        Texture2D[,] textures = await LoadEveryTexture();

        await AsyncLoadScene(0);

        MapManager.instence.CreateChunck(meshs, textures);
        MapManager.instence.enabled = true;
    }

    public async Task<Texture2D[,]> LoadEveryTexture()
    {
        Texture2D[,] chunks = new Texture2D[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                LoadingIndicator = $"Loading Texture {y * Height + x}/{Height*Width}";
                chunks[x, y] = await TextureGenerator.AsyncGetChunkTexture(new Vector2Int(x, y), MapManager.map);
            }
        }
        return chunks;
    }

    public async Task<Mesh[,]> LoadEveryMesh()
    {
        Mesh[,] chunks = new Mesh[Width, Height];
        await Task.Delay(1);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                LoadingIndicator = $"Loading Mesh {y * Height + x}/{Height * Width}";
                chunks[x, y] = await MeshGenerator.AsyncGetChunkMesh(new Vector2Int(x, y), MapManager.map);
                if ((y * Height + x) % 50 == 0)
                {
                    await Task.Delay(1);
                }
            }
        }
        await Task.Delay(1);
        return chunks;
    }

    public async Task AsyncLoadScene(int index)
    {
        AsyncOperation loadMap = SceneManager.LoadSceneAsync(index);
        while (!loadMap.isDone)
        {
            await AsyncTask.DelayIfNeed(1);
        }
    }

    private void Update()
    {
        if (operation != null && load == LoadStatus.Loading && operation.Status == TaskStatus.Faulted)
        {
            load = LoadStatus.Faulted;
            Debug.LogError($"message: {operation.Exception.Message}, \n source: {operation.Exception.Source}, \n\n comple: {operation.Exception}");
        }
    }

    public enum LoadStatus
    {
        NotStart,
        Loading,
        Done,
        Faulted
    }
}
