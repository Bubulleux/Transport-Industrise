using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using System;

public class VehicleContoler : MonoBehaviour
{
    public VehicleData vehicleData;
    public float damage = 0f;
    public Route route;
    public int routePointGo;
    public List<Vector2Int> path = new List<Vector2Int>();
    public int id;
    public Vector2Int vehiclePos { get { return transform.position.ToVec2Int(); } }
    void Start()
    {
        id = GameObject.FindGameObjectsWithTag("Vehicle").Length;
        transform.parent = GameObject.Find("Vehicles").transform;
    }

    void Update()
    {
        if (route != null && (path == null || path.Count == 0))
        {
            path = PathFinder.FindPath(vehiclePos, route.points[routePointGo]);
            foreach (Vector2Int curPathPoint in path)
            {
                Debug.Log(curPathPoint);
                MapManager.map.Color(curPathPoint, Color.cyan);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log($"Route: {route == null}, path {path.Count == 0}");
        }
        Vector2Int pos = transform.position.ToVec2Int();
        float y = MapManager.map.parcels[pos.x, pos.y].corner.Max();
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public enum VehicleStat
    {
        InDepot,
        OnWay,
        BroekenDown,
        OnDepotWay,
        Stuck
    }
}
public static class Vector3Extension
{
    public static Vector2Int ToVec2Int(this Vector3 vec)
    {
        return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
    }
}