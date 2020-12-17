using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using System;
using System.Threading.Tasks;

public class VehicleContoler : MonoBehaviour
{
    public VehicleData vehicleData;
    public int id;
    public float damage = 0f;

    public Route route;
    public int routePointGo;
    public List<Vector2Int> path = new List<Vector2Int>();
    public Groupe groupe = null;

    //private float moveCooldown = 0f;
    public Animator vehicleAnimation;

    public VehicleStat state = VehicleStat.InDepot;

    public Vector2Int VehiclePos { get { return transform.position.ToVec2Int(); } 
        set
        {
            float y = MapManager.map.parcels[value.x, value.y].corner.Max();
            transform.position = new Vector3(value.x, y, value.y);
        } }

    public Materials materialCurTransport;
    public int materialQuantity;

    public Task asyncUpdateOperation;
    void Start()
    {
        id = GameObject.FindGameObjectsWithTag("Vehicle").Length;
        transform.parent = GameObject.Find("Vehicles").transform;
        asyncUpdateOperation = AsyncUpdate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log(state);
        }
        
        if (asyncUpdateOperation.Status == TaskStatus.Faulted)
        {
            Debug.LogError($"message: {asyncUpdateOperation.Exception.Message}, \n source: {asyncUpdateOperation.Exception.Source}, \n\n comple: {asyncUpdateOperation.Exception.ToString()}");
            asyncUpdateOperation = AsyncUpdate();
        }

        Vector2Int pos = transform.position.ToVec2Int();
    }

    private async Task AsyncUpdate()
    {
        while(true)
        {
            await Task.Delay(10);
            if (route == null && state != VehicleStat.InDepot && state != VehicleStat.OnDepotWay)
            {
                ReturnInDepot();
            }

            //if (state == VehicleStat.InDepot || state == VehicleStat.OnDepotWay)
            //{
            //    continue;
            //}

            if (state == VehicleStat.OnWay)
            {
                if (VehiclePos == route.points[routePointGo])
                {
                    GoToAnotherPoint();
                    path = PathFinder.FindPath(VehiclePos, route.points[routePointGo]);
                    if (MapManager.map.GetparcelType(VehiclePos) == typeof(LoadingBay) && CanDoSomthing(VehiclePos) != Action.nothing)
                    {
                        state = VehicleStat.Loading;
                        await Load();
                        Debug.Log($"material Quantity: {materialQuantity}");
                        Debug.Log($"On WAY: {route.points[routePointGo]}");
                        state = VehicleStat.OnWay;
                    }
                }
            }
            if (path != null && path.Count != 0)
            {
                await DriveAlong();
            }
        }
    }

    public void GoToAnotherPoint()
    {
        Debug.Log("Change Route point");
        Debug.Log($"CurenteMaterial : {materialCurTransport}, quantity: {materialQuantity}");
        for (int i = 1; i < route.points.Count; i++)
        {
            if (CanDoSomthing(route.points[(routePointGo + i) % route.points.Count]) != Action.nothing)
            {
                routePointGo = (routePointGo + i) % route.points.Count;
                return;
            }
        }
        routePointGo += 1;
        routePointGo %= route.points.Count;
    }

    public async Task StartVehicle()
    {
        Debug.Log("StartVehicle");
        if (route == null)
        {
            return;
        }
        path = PathFinder.FindPath(VehiclePos, route.points[routePointGo]);
        vehicleAnimation.Play("Start", 0, 0f);
        await Task.Delay(Mathf.FloorToInt(1000f / vehicleData.speed));
        state = VehicleStat.OnWay;
    }

    public async Task DriveAlong()
    {
        await Task.Delay(Mathf.FloorToInt(1000f / vehicleData.speed));
        AnimeVehicle();
        VehiclePos = path[0];
        path.RemoveAt(0);
    }

    public void ReturnInDepot()
    {
        if (MapManager.map.parcels[VehiclePos.x, VehiclePos.y].GetType() == typeof(Depot) || state == VehicleStat.InDepot)
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
        state = VehicleStat.OnDepotWay;
        path = PathFinder.FindPath(VehiclePos, depotsPos);
        if (path == null)
        {
            state = VehicleStat.Stuck;
            return;
        }
        Debug.Log($"Path count to go depot: {path.Count}");
    }

    private async Task Load()
    {
        Debug.Log("Load");
        if (materialQuantity != 0 && CanDoSomthing(VehiclePos) == Action.unload)
        {
            int materialSucessful = MapManager.map.GetParcel<LoadingBay>(VehiclePos).TryToInteract(materialCurTransport, materialQuantity);
            materialQuantity -= materialSucessful;
            await Task.Delay(2000);
            return;
        }
        if (materialQuantity == 0 && CanDoSomthing(VehiclePos) == Action.load)
        {
            foreach(Materials curMaterial in vehicleData.materialCanTransport)
            {
                if (MapManager.map.GetParcel<LoadingBay>(VehiclePos).GetMaterialOutpute()[curMaterial] != 0)
                {
                    int materialSucessful = MapManager.map.GetParcel<LoadingBay>(VehiclePos).TryToInteract(materialCurTransport, materialQuantity - vehicleData.maxMaterialTransport);
                    materialQuantity -= materialSucessful;
                    await Task.Delay(2000);
                    return;
                }
            }
        }
    }

    public Action CanDoSomthing(Vector2Int pos)
    {
        Type parcelType = MapManager.map.GetparcelType(pos);
        if (parcelType == typeof(LoadingBay))
        {
            LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(pos);
            if (materialQuantity == 0)
            {
                foreach (Materials curMaterialVehicle in vehicleData.materialCanTransport)
                {
                    if (loadingBay.GetMaterial(curMaterialVehicle) > 0)
                    {
                        return Action.load;
                    }
                }
            }
            else
            {
                if (loadingBay.CanUnload(materialCurTransport))
                {
                    return Action.unload;
                }
            }
        }
        else if (parcelType == typeof(Depot))
        {
            if (damage >= 0.8f)
            {
                return Action.repare;
            }
        }
        return Action.nothing;
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

    

    public enum VehicleStat
    {
        InDepot,
        OnWay,
        BroekenDown,
        OnDepotWay,
        Stuck,
        Loading
    }

    public enum Action
    {
        load,
        unload,
        repare,
        nothing
    }
}
public static class Vector3Extension
{
    public static Vector2Int ToVec2Int(this Vector3 vec)
    {
        return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
    }
}