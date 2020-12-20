using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleWIndow : Window
{
    public VehicleContoler vehicle;

    private void Start()
    {
        UpdateWindow();
    }

    public void UpdateWindow()
    {
        Contente.Find("Name").GetComponent<Text>().text = vehicle.vehicleData.name;
        Contente.Find("Damage").GetComponent<Text>().text = "Damage:" + vehicle.damage.ToString();
        Contente.Find("ID").GetComponent<Text>().text = "ID: " + vehicle.Id;
        Contente.Find("Route").GetComponent<Button>().interactable = (vehicle.MyGroup == null || vehicle.MyGroup.forceRoute == false);
        Dropdown group = Contente.Find("Group").GetComponent<Dropdown>();
        List<string> options = new List<string>();
        int index = Group.groups.Count;
        foreach(Group curGroup in Group.groups)
        {
            options.Add(curGroup.name);
            if (vehicle.MyGroup == curGroup)
            {
                index = options.Count - 1;
            }
        }
        options.Add("None");
        group.ClearOptions();
        group.AddOptions(options);
        group.SetValueWithoutNotify(index);
    }

    public void GroupSet(int index)
    {
        if (index == Group.groups.Count)
        {
            vehicle.MyGroup = null;
        }
        else
        {
            vehicle.MyGroup = Group.groups[index];
        }
        UpdateWindow();
    }

    public void SetRoute()
    {
        WindowsOpener.OpenRouteCreatorWindow(delegate (Route route) { vehicle.MyRoute = route; }, vehicle.MyRoute);
    }
}
