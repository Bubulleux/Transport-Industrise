using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleWIndow : MonoBehaviour
{
    public VehicleContoler vehicle;
    public Transform vehicleInfo;
    public bool first = true;
    void Start()
    {
        vehicleInfo.Find("Name").GetComponent<Text>().text = vehicle.vehicleData.name;
        vehicleInfo.Find("Damage").GetComponent<Text>().text = "Damage:" + vehicle.damage.ToString();
        vehicleInfo.Find("ID").GetComponent<Text>().text = "ID: " + vehicle.id;
        Dropdown routeDropdown = vehicleInfo.Find("RouteDropdown").GetComponent<Dropdown>();
        List<string> options = new List<string>();
        int index = -1;
        if (vehicle.Route == null)
        {
            index = RoutesListWindow.routes.Count + 1;
        }
        foreach(Route curRoute in RoutesListWindow.routes)
        {
            options.Add(curRoute.name);
            if (vehicle.Route == curRoute)
            {
                index = options.Count - 1;
            }
        }
        options.Add("Custom");
        options.Add("None");
        if (index == -1)
        {
            index = RoutesListWindow.routes.Count;
        }
        routeDropdown.AddOptions(options);
        routeDropdown.value = index;
    }

    public void RouteSet(int index)
    {
        if (first)
        {
            first = false;
            return;
        }
        if (index == RoutesListWindow.routes.Count)
        {
            WindowsOpener.OpenRouteCreatorWindow(delegate(Route route){ vehicle.Route = route; });
        }
        else if (index == RoutesListWindow.routes.Count + 1)
        {
            vehicle.Route = null;
        }
        else
        {
            vehicle.Route = RoutesListWindow.routes[index];
        }
    }
}
