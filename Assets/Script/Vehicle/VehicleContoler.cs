using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using System;

public class VehicleContoler : MonoBehaviour
{
    public VehicleData vehicleData;
    public int id;
    public float damage = 0f;

    public Route route;
    public int routePointGo;
    public List<Vector2Int> path = new List<Vector2Int>();

    private float moveCooldown = 0f;
    public Animator vehicleAnimation;

    public VehicleStat state;

    public Vector2Int VehiclePos { get { return transform.position.ToVec2Int(); } set { transform.position = new Vector3(value.x, 0f, value.y); } }

    public Materials materialCurTransport;
    public float meterialQuantity;
    void Start()
    {
        id = GameObject.FindGameObjectsWithTag("Vehicle").Length;
        transform.parent = GameObject.Find("Vehicles").transform;
        state = VehicleStat.InDepot;
    }

    void Update()
    {
        
        if (route == null && state != VehicleStat.InDepot && state != VehicleStat.OnDepotWay)
        {
            ReturnInDepot();
        }

        if (state == VehicleStat.OnWay)
        {
            if (VehiclePos == route.points[routePointGo])
            {
                routePointGo += 1;
                routePointGo %= route.points.Count;
                path = PathFinder.FindPath(VehiclePos, route.points[routePointGo]);
                state = VehicleStat.Loading;
            }
        }

        if (path != null && path.Count != 0)
        {
            DriveAlong();
        }
        Vector2Int pos = transform.position.ToVec2Int();
        float y = MapManager.map.parcels[pos.x, pos.y].corner.Max();
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (moveCooldown > 0)
        {
            moveCooldown -= Time.fixedDeltaTime;
        }
    }

    public void DriveAlong()
    {
        if (VehiclePos == path[0])
        {
            path.RemoveAt(0);
        }
        else if (moveCooldown <= 0)
        {
            AnimeVehicle();
            VehiclePos = path[0];
            path.RemoveAt(0);
            moveCooldown = 1f / vehicleData.speed;
        }
    }

    private void ReturnInDepot()
    {
        if (MapManager.map.parcels[VehiclePos.x, VehiclePos.y].GetType() == typeof(Depot) || state == VehicleStat.InDepot || state == VehicleStat.OnDepotWay)
        {
            state = VehicleStat.InDepot;
            return;
        }
        Depot[] depotsList = MapManager.map.GetImpotantParcel<Depot>();
        Vector2Int[] depotsPos = new Vector2Int[depotsList.Length];
        for (int i = 0; i < depotsPos.Length; i++)
        {
            depotsPos[i] = depotsList[i].pos;
        }
        path = PathFinder.FindPath(VehiclePos, depotsPos);
        if (path == null)
        {
            state = VehicleStat.Stuck;
            return;
        }
    }

    public bool CanDoSomthing(Vector2Int pos)
    {
        Type parcelType = MapManager.map.parcels[pos.x, pos.y].GetType();
        if (parcelType == typeof(LoadingBay))
        {
            LoadingBay loadingBay = MapManager.map.parcels[pos.x, pos.y] as LoadingBay;
            if (materialCurTransport == 0)
            {
                foreach(Industrise curIndustrise in loadingBay.industriseLink)
                {
                    foreach(Materials curMaterialIndu in curIndustrise.materialsOutpute)
                    {
                        foreach(Materials curMaterialVehicle in vehicleData.materialCanTransport)
                        {
                            if ()
                        }
                    }
                }
            }
        }
        return false;
    }

    public void AnimeVehicle()
    {
        
        if (path.Count <= 1)
        {
            return;
        }
        Vector2Int posBefor = VehiclePos - path[0];
        Vector2Int posGo = path[1] - path[0];
        float dir = Mathf.Atan2(posBefor.x * -1, posBefor.y * -1) * Mathf.Rad2Deg;
        vehicleAnimation.transform.rotation = Quaternion.Euler(0f, dir, 0f);
        int dirAnime = 0;
        if (posBefor == new Vector2Int(0,1))
        {
            dirAnime = posGo == new Vector2Int(-1, 0) ? 1 : posGo == new Vector2Int(1, 0) ? -1 : 0;
        }
        else if (posBefor == new Vector2Int(1, 0))
        {
            dirAnime = posGo == new Vector2Int(0, 1) ? 1 : posGo == new Vector2Int(0, -1) ? -1 : 0;
        }
        else if (posBefor == new Vector2Int(0, -1))
        {
            dirAnime = posGo == new Vector2Int(1, 0) ? 1 : posGo == new Vector2Int(-1, 0) ? -1 : 0;
        }
        else if (posBefor == new Vector2Int(-1, 0))
        {
            dirAnime = posGo == new Vector2Int(0, -1) ? 1 : posGo == new Vector2Int(0, 1) ? -1 : 0;
        }
        //float dirAng = Mathf.Atan2(posGo.x, posGo.y) - Mathf.Atan2(posBefor.x, posBefor.y);
        //dirAnime = Mathf.FloorToInt(Mathf.Sin(dirAng));
        vehicleAnimation.StopPlayback();
        vehicleAnimation.Play(dirAnime == 0 ? "Forward" : dirAnime == -1 ? "TurnRight" : "TurnLeft", 0, 0f);
        //vehicleAnimation.SetInteger("Direction", dirAnime);
        vehicleAnimation.SetFloat("Speed", vehicleData.speed);
    }

    public void StartVehicle()
    {
        if(route == null)
        {
            return;
        }
        path = PathFinder.FindPath(VehiclePos, route.points[routePointGo]);
        moveCooldown = 1 / vehicleData.speed;
        state = VehicleStat.OnWay;
        vehicleAnimation.Play("Start", 0, 0f);
    }

    public enum VehicleStat
    {
        InDepot,
        OnWay,
        BroekenDown,
        OnDepotWay,
        Stuck,
        Loading
    }
}
public static class Vector3Extension
{
    public static Vector2Int ToVec2Int(this Vector3 vec)
    {
        return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
    }
}