using System.Threading.Tasks;
using Script.MapGeneration;
using Script.Mapping;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Game
{
    public class GameLoader : MonoBehaviour
    {
        public static GameLoader instence;
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
            AsyncTask.MonitorTask(instence.AsyncCreatGame(saveName));
        }

        public static void LoadSave(string saveName)
        {
            AsyncTask.MonitorTask(instence.AsyncLoadSave(saveName));
        }

        public async Task AsyncCreatGame(string saveName)
        {
            await AsyncLoadScene(1);

            LoadingIndicator = "Generate Map";
            Map _mapdata = new Map(new Vector2Int(20, 20));
            await _mapdata.GenerateMap(FIleSys.GetAllInstances<MapSettingData>()[0]);
            MapManager.map = _mapdata;
            GameManager.saveName = saveName;

            await LoadGame();

            //new Save().SaveGame();

            load = LoadStatus.Done;
        }

        public async Task AsyncLoadSave(string saveName)
        {
            await AsyncLoadScene(1);
            LoadingIndicator = "Load Save: " + saveName;
            Save.Save save = new Save.Save(saveName);
            await Task.Delay(1);
            await AsyncTask.MonitorTask(save.LoadGame());

            MapManager.map = save.map;
            GameManager.saveName = save.name;
            await LoadGame();
            save.LoadGameSeconde();
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
            Debug.Log("Load Texture");
            Texture2D[,] chunks = new Texture2D[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    LoadingIndicator = $"Loading Texture {y * Height + x}/{Height*Width}";
                    chunks[x, y] = TextureGenerator.GetChunkTexture(new Vector2Int(x, y), MapManager.map);
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
                    chunks[x, y] = MeshGenerator.GetChunkMesh(new Vector2Int(x, y), MapManager.map);
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

        }

        public enum LoadStatus
        {
            NotStart,
            Loading,
            Done,
            Faulted
        }
    }
}
