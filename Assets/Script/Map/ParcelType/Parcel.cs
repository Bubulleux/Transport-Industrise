using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class Parcel
{
    public Vector2Int pos;
    public int[] corner = new int[4];
    //public object construction = null;
    public bool seeTerrain = true;
    public Parcel parent;

    public Color32 color = Color.green;

    public bool IsInWater { get { return corner[0] < 1 || corner[1] < 1 || corner[2] < 1 || corner[3] < 1; } }

    public Parcel(Vector2Int _pos)
    {
        pos = _pos;
    }
    public Parcel() { }

    public virtual void Initialaze()
    {

    }

    public virtual void Interact()
    {

    }

    public virtual void DebugParcel()
    {
        UnityEngine.Debug.Log(pos);
        string parcelJson = Save.GetJson(MapManager.map.GetParcel(pos));
        Debug.Log(parcelJson);
    }

    public static Parcel CopyClass(Parcel copyClass, Parcel pastClass)
    {
        pastClass.pos = copyClass.pos;
        pastClass.corner = copyClass.corner;
        pastClass.seeTerrain = copyClass.seeTerrain;
        pastClass.color = copyClass.color;
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
