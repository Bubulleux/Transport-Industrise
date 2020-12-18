using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowsOpener 
{
    public static GameObject OpenDepotWindow(Vector2Int pos)
    {
        GameObject _go = OpenWindowByName("DepotWindow");
        _go.GetComponent<DepotWindow>().Init(pos);
        return _go;
    }

    public static GameObject OpenRouteListWindow()
    {
        GameObject _go = OpenWindowByName("RoutesListWindow");
        return _go;
    }
    public static GameObject OpenGroupesListWindow()
    {
        return OpenWindowByName("GroupesListWindow");
    }

    public static GameObject OpenRouteCreatorWindow(RouteCreatorWindow.FunctionFinish _functionFinish, Route _route = null)
    {
        GameObject _go = OpenWindowByName("RoutesCreatorWindow");
        _go.GetComponent<RouteCreatorWindow>().functionFinish = _functionFinish;
        if (_route != null)
        {
            _go.GetComponent<RouteCreatorWindow>().route = _route;
        }
        return _go;
    }

    public static GameObject OpenVehicleWindow(VehicleContoler vehicle)
    {
        GameObject _go = OpenWindowByName("VehicleWindow");
        _go.GetComponent<VehicleWIndow>().vehicle = vehicle;
        return _go;
    }

    public static GameObject OpenGroupWindow(Group group)
    {
        GameObject _go = OpenWindowByName("GroupWindow");
        _go.GetComponent<GroupWindow>().group = group;
        return _go;
    }

    private static GameObject OpenWindowByName(string name)
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/" + name, typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        return _go;
    }
}
