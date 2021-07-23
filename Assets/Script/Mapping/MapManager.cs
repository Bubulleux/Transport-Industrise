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

    
        public Chuck[,] ChuckObjects = new Chuck[20, 20];
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
            for (int y = 0; y < ChuckObjects.GetLength(0); y++)
            {
                for (int x = 0; x < ChuckObjects.GetLength(0); x++)
                { 
                    ChuckObjects[x, y] = Instantiate(gfxMapPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Chunks").transform).GetComponent<Chuck>(); 
                    ChuckObjects[x, y].Initialize(new Vector2Int(x, y), meshs[x, y], texrures[x, y]);
                }
            }
        }

        private void Update()
        {
            foreach (Parcel curParcel in map.updatedParcel)
                curParcel.Update();
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
                    ChuckObjects[x, y].UpdateChuck(true);
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
                    ChuckObjects[x, y].UpdateChuck(false);
                }
            }
            if (mapHasBeenUpdate)
            {
                MapUpdateEvent?.Invoke();
            }
        }

    }
}