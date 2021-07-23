using Newtonsoft.Json;
using Script.MapGeneration;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Parcel
    {
        public Vector2Int pos;
        public int[] corner = new int[4];
        //public object construction = null;
        public bool seeTerrain = true;
        public Parcel parent;
        public BiomeData biome;

        public Color32 color =  Color.magenta;
        public Mesh ObjectMesh = null;
        public Material[] MeshMaterial = null;
        public Vector3 ObjectPosition = Vector3.zero;
        public Quaternion ObjectRotation = Quaternion.Euler(Vector3.zero);
        public Map map;

        public bool IsInWater { get { return corner[0] < 1 || corner[1] < 1 || corner[2] < 1 || corner[3] < 1; } }

        public Parcel(Vector2Int _pos, Map _map)
        {
            pos = _pos;
            map = _map;
        }
        public Parcel() { }
    
        public virtual void Initialaze()
        {
            color = biome.color;
        }

        public virtual void Interact()
        {

        }

        public virtual void DebugParcel()
        {
            Debug.Log($"{pos} {corner[0]}, {corner[1]}, {corner[2]}, {corner[3]}");
            
            //string parcelJson = Save.Save.GetJson(MapManager.map.GetParcel(pos));
            //Debug.Log(parcelJson);
        }

        public static Parcel CopyClass(Parcel copyClass, Parcel pastClass)
        {
            pastClass.pos = copyClass.pos;
            pastClass.corner = copyClass.corner;
            pastClass.seeTerrain = copyClass.seeTerrain;
            pastClass.color = copyClass.color;
            pastClass.map = copyClass.map;
            pastClass.biome = copyClass.biome;
            pastClass.Initialaze();
            return pastClass;
        }

        public void EnableUpdate(bool enable)
        {
            if (enable && !MapManager.map.updatedParcel.Contains(this))
                MapManager.map.updatedParcel.Add(this);
            else if (!enable && MapManager.map.updatedParcel.Contains(this))
                MapManager.map.updatedParcel.Remove(this);
        }

        public virtual void Update() { }
    }
}
