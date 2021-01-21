using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepotWindow : WindowContent
{
    public GameObject templateDepotVehicle;
    public GameObject templateStoreVehicle;
    public Transform listContente;
    public Depot depot;
    public bool onStore = false;
    public Text butTxt;
    public Dropdown groupesDropdown;

    private void Start()
    {
        WindowParente.WindowName = "Depot";
        UpdateWindow();
    }

    private void Update()
    {
        if (!onStore && VehicleManager.GetVehicleByPos(depot).Count != listContente.childCount - 2)
        {
            UpdateWindow();
        }
    }

    public void UpdateWindow()
    {
        foreach(Transform child in listContente)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
        if (onStore)
        {
            groupesDropdown.gameObject.SetActive(true);
            List<string> dropdownOption = new List<string>();
            foreach(Group curGroupe in Group.groups)
            {
                dropdownOption.Add(curGroupe.name);
            }
            dropdownOption.Add("None");
            groupesDropdown.ClearOptions();
            groupesDropdown.AddOptions(dropdownOption);
            groupesDropdown.value = Group.groups.Count;
            foreach (VehicleData curVehicle in FIleSys.GetAllInstances<VehicleData>())
            {
                Transform _go = Instantiate(templateStoreVehicle).transform;
                _go.SetParent(listContente);
                _go.Find("Name").GetComponent<Text>().text = curVehicle.name;
                _go.Find("Description").GetComponent<Text>().text = curVehicle.description;
                _go.Find("Buy").GetComponent<Button>().onClick.AddListener(delegate 
                { 
                    VehicleContoler vehicle = depot.BuyVehicle(curVehicle);
                    if (groupesDropdown.value != Group.groups.Count)
                    {
                        vehicle.MyGroup = Group.groups[groupesDropdown.value];
                    }
                });
                _go.Find("Buy").GetComponent<ButtonInteractMoney>().condiction = delegate()
                {
                    return GameManager.Money >= curVehicle.price;
                };
                _go.gameObject.SetActive(true);
            }
        }
        else
        {
            groupesDropdown.gameObject.SetActive(false);
            foreach (VehicleContoler curVehicle in VehicleManager.GetVehicleByPos(depot))
            {
                Transform _go = Instantiate(templateDepotVehicle).transform;
                _go.SetParent(listContente);
                _go.Find("Name").GetComponent<Text>().text = curVehicle.vehicleData.name;
                _go.Find("Damage").GetComponent<Text>().text = string.Format("Damage: {0}%", Mathf.Floor(curVehicle.damage * 100));
                _go.Find("ID").GetComponent<Text>().text = "ID: " + curVehicle.Id;
                _go.Find("Info").GetComponent<Button>().onClick.AddListener(delegate { WindowsOpener.OpenVehicleWindow(curVehicle.GetComponent<VehicleContoler>()); });
                _go.gameObject.SetActive(true);
            }
        }
        templateDepotVehicle.SetActive(false);
        templateStoreVehicle.SetActive(false);
    }


    public void Store()
    {
        onStore = !onStore;
        butTxt.text = onStore ? "Depot" : "Store";
        UpdateWindow();
    }

    public void StartVehicle()
    {
        foreach(VehicleContoler curVehicle in VehicleManager.GetVehicleByPos(depot))
        {
            curVehicle.StartVehicle();
        }
    }
    
}
