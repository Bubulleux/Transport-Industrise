using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Parcel
{
    public Vector2Int pos;
    public int[] corner = new int[4];
    //public object construction = null;
    public bool seeTerrain = true;

    public virtual void Initialaze()
    {

    }

    public static Parcel CopyClass(Parcel copyClass,Parcel pastClass)
    {
        pastClass.pos = copyClass.pos;
        pastClass.corner = copyClass.corner;
        pastClass.seeTerrain = copyClass.seeTerrain;
        pastClass.Initialaze();
        return pastClass;
    }

}

public class Road : Parcel
{
    public bool[] direction = new bool[4];
}
public class Building : Parcel
{
    public float height;
    public Color color;
}



public class Depot : Parcel
{

    public void BuyVehicle(VehicleData vehicle)
    {
        GameObject _go = Object.Instantiate(Resources.Load("Vehicle") as GameObject);
        _go.transform.position = new Vector3(pos.x , 0f, pos.y);
        _go.GetComponent<VehicleContoler>().vehicleData = vehicle;
    }

    public List<GameObject> GetVehicles()
    {
        List<GameObject> _return = new List<GameObject>();
        foreach(GameObject _go in GameObject.FindGameObjectsWithTag("Vehicle"))
        {
            if (_go.transform.position.ToVec2Int() == pos)
            {
                _return.Add(_go);
            }
        }
        return _return;
    }
}

public class LoadingBay : Parcel
{
    public List<Industrise> industriseLink = new List<Industrise>();
    public override void Initialaze()
    {
        foreach(Industrise curIndustrise in MapManager.map.industrises)
        {
            if (Vector2Int.Distance(pos, curIndustrise.MasterPos) <= 20)
            {
                industriseLink.Add(curIndustrise);
            }
        }
        Debug.Log($"Industrise link: {industriseLink.Count}");
    }
}