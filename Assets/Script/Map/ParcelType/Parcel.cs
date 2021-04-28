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
        pastClass.Initialaze();
        return pastClass;
    }

}
