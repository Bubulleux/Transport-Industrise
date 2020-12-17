using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindosOpener 
{
    public static GameObject OpenDepotWindow(Vector2Int pos)
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/DepotWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        _go.GetComponent<DepotWindow>().Init(pos);
        return _go;
    }

    public static GameObject OpenRouteListWindow()
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/RoutesListWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        return _go;
    }
    public static GameObject OpenGroupesListWindow()
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/GroupesListWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        return _go;
    }

    public static GameObject OpenRouteCreatorWindow(RouteCreatorWindow.FunctionFinish _functionFinish, Route _route = null)
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/RoutesCreatorWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        _go.GetComponent<RouteCreatorWindow>().functionFinish = _functionFinish;
        if (_route != null)
        {
            _go.GetComponent<RouteCreatorWindow>().route = _route;
        }
        return _go;
    }

    public static GameObject OpenVehicleWindow(VehicleContoler vehicle)
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/VehicleWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        _go.GetComponent<VehicleWIndow>().vehicle = vehicle;
        return _go;
    }
}
