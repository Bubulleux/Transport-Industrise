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
    public List<Vector2Int> path;
    void Start()
    {
        transform.parent = GameObject.Find("Vehicles").transform;
    }

    void Update()
    {
        Vector2Int pos = transform.position.ToVec2Int();
        float y = Map.instence.parcels[pos.x, pos.y].corner.Max();
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
public static class Vector3Extension
{
    public static Vector2Int ToVec2Int(this Vector3 vec)
    {
        return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
    }
}