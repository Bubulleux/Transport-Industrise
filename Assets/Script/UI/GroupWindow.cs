using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupWindow : Window
{
    public Group group;

    public void Start()
    {
        UpdateWindow();
    }
    public void UpdateWindow()
    {
        contente.Find("Name").GetComponent<InputField>().text = group.name;
        contente.Find("VehicleCount").GetComponent<Text>().text = "Vehicle Count: " + group.vehicles.Count;
        contente.Find("ForceRoute").GetComponent<Toggle>().isOn = group.forceRoute;
        contente.Find("SetRouteBut").GetComponent<Button>().interactable = group.forceRoute;
    }

    public void SetRoute()
    {
        WindowsOpener.OpenRouteCreatorWindow(delegate (Route route)
        {
            group.route = route;
            UpdateWindow();
        }, group.route);
    }

    public void SetForceRoute(bool force)
    {
        group.forceRoute = force;
        UpdateWindow();
    }

    public void SetName(string name)
    {
        group.name = name;
    }
}
