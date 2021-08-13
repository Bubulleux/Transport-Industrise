using System.Collections.Generic;
using Script.Game;
using Script.Mapping.ParcelType;
using UnityEngine;

namespace Script.Mapping
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instence;
        public static ParcelSelector Selector;

        public GameObject roadPrefab;
        public GameObject buildingPrefab;
        //public float[,] mapHeight;

    
        public Chuck[,] ChuckObjects = new Chuck[map.Size.x / Map.ChuckSize, map.Size.y / Map.ChuckSize];
        public GameObject gfxMapPrefab;

        private Dictionary<City, float> cityDeltaTime = new Dictionary<City, float>();
        private Dictionary<FactoryParcel, float> factoryDeltaTime = new Dictionary<FactoryParcel, float>();


        public static Map map;
        public delegate void MapUpdateEventDelegate();
        public event MapUpdateEventDelegate MapUpdateEvent;

        public static readonly Vector2Int[] parcelAround =
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
        };
        
        public static readonly Vector2Int[] cornerAround =
        {
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, 0),
            new Vector2Int(0, 1),
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
            Selector = GetComponent<ParcelSelector>();
        }
        
        public void CreateChunck(Mesh[,] meshs, Texture2D[,] texrures)
        {
            map.Manager = this;
            for (int y = 0; y < ChuckObjects.GetLength(1); y++)
            {
                for (int x = 0; x < ChuckObjects.GetLength(0); x++)
                { 
                    ChuckObjects[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform).GetComponent<Chuck>(); 
                    ChuckObjects[x, y].Initialize(new Vector2Int(x, y), meshs[x, y], texrures[x, y]);
                }
            }
            Debug.Log($"{ChuckObjects.Length} Chuck Instentiate");
            InstanceAllGFX();
        }

        private void InstanceAllGFX()
        {
            for (int y = 0; y < map.Size.y; y++)
            {
                for (int x = 0; x < map.Size.x; x++)
                {
                    if (map.GetParcel(new Vector2Int(x, y)).prefab != null)
                    {
                        InstenceGFX(map.GetParcel(new Vector2Int(x, y)));
                    }
                }
            }
        }
        
        
        private void Update()
        {
            foreach (Parcel curParcel in map.updatedParcel)
                curParcel.Update();
            if (GameLoader.load == GameLoader.LoadStatus.Done)
            {
                float deltaTime = TimeManager.DeltaTime;
                
                City cityUpdate = map.citys[0];
                foreach (var city in map.citys)
                {
                    if (!cityDeltaTime.ContainsKey(city))
                        cityDeltaTime.Add(city, 0f);
                    
                    cityDeltaTime[city] += deltaTime;
                    if (cityDeltaTime[city] > cityDeltaTime[cityUpdate])
                        cityUpdate = city;
                }
                
                FactoryParcel factoryUpdate = map.factories[0];
                foreach (var factory in map.factories)
                {
                    if (!factoryDeltaTime.ContainsKey(factory))
                        factoryDeltaTime.Add(factory, 0f);
                    
                    factoryDeltaTime[factory] += deltaTime;
                    if (factoryDeltaTime[factory] > factoryDeltaTime[factoryUpdate])
                        factoryUpdate = factory;
                }
                
                cityUpdate.Update(cityDeltaTime[cityUpdate]);
                cityDeltaTime[cityUpdate] = 0f;
                
                factoryUpdate.UpdateProduction(factoryDeltaTime[factoryUpdate]);
                factoryDeltaTime[factoryUpdate] = 0f;
            }
        }

        void OnPreRender()
        {
            UpdateMap();
        }

        public void ResetGFX(Parcel parcel)
        {
            DestroyGFX(parcel);
            InstenceGFX(parcel);
        }
        
        public void InstenceGFX(Parcel parcel)
        {
            if (parcel.prefab == null)
                return;
            var go = Instantiate(parcel.prefab);
            parcel.gfx = go;
            if (go.TryGetComponent(out ParcelGFX gfxScript))
            {
                gfxScript.parcel = parcel;
                var pos = GetChuckPos(parcel.pos);
                ChuckObjects[pos.x, pos.y]._gfx.Add(parcel.pos, go);
                go.transform.parent = ChuckObjects[pos.x, pos.y].transform;
                gfxScript.UpdateGFX();
            }
        }

        
        public void DestroyGFX(Parcel parcel)
        {
            var pos = GetChuckPos(parcel.pos);
            if (ChuckObjects[pos.x, pos.y]._gfx.ContainsKey(parcel.pos))
            {
                parcel.gfx = null;
                Destroy(ChuckObjects[pos.x, pos.y]._gfx[parcel.pos]);
                ChuckObjects[pos.x, pos.y]._gfx.Remove(parcel.pos);
            }
        }
        
        public static Vector2Int GetChuckPos(Vector2Int pos)
        {
            return new Vector2Int(Mathf.FloorToInt(pos.x / (float) Map.ChuckSize),
                Mathf.FloorToInt(pos.y / (float) Map.ChuckSize));
        }
        
        public void UpdateEveryChunck()
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    ChuckObjects[x, y].UpdateChuck(true);
                }
            }
        }

        public void UpdateMap()
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    ChuckObjects[x, y].UpdateChuck(false);
                }
            }
        }

    }
}