using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road
{
    public bool[] direction = new bool[4];
}
public class Building
{
    public float height;
    public Color color;
}

public class Depot
{
    public Vector2Int pos;
    
    public Depot(Vector2Int _pos)
    {
        pos = _pos;
    }

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