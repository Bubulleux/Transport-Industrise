using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepotWindow : MonoBehaviour
{
    public GameObject templateDepotVehicle;
    public GameObject templateStoreVehicle;
    public Transform listContente;
    private Vector2Int depotpos;
    public Depot Depot { set => MapManager.map.parcels[depotpos.x, depotpos.y] = value;  get => (Depot)MapManager.map.parcels[depotpos.x, depotpos.y];}
    public bool onStore = false;
    public Text butTxt;
    public Dropdown groupesDropdown;
    public void Init(Vector2Int _depot)
    {
        depotpos = _depot;
        UpdateWindow();
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
            foreach(Groupe curGroupe in Groupe.groupes)
            {
                dropdownOption.Add(curGroupe.name);
            }
            dropdownOption.Add("None");
            groupesDropdown.ClearOptions();
            groupesDropdown.AddOptions(dropdownOption);
            groupesDropdown.value = Groupe.groupes.Count;
            foreach (VehicleData curVehicle in FIleSys.GetAllInstances<VehicleData>())
            {
                Transform _go = Instantiate(templateStoreVehicle).transform;
                _go.SetParent(listContente);
                _go.Find("Name").GetComponent<Text>().text = curVehicle.name;
                _go.Find("Description").GetComponent<Text>().text = curVehicle.description;
                _go.Find("Buy").GetComponent<Button>().onClick.AddListener(delegate 
                { 
                    VehicleContoler vehicle = Depot.BuyVehicle(curVehicle);
                    if (groupesDropdown.value != Groupe.groupes.Count)
                    {
                        Debug.Log("vehicle Groupe Set");
                        vehicle.groupe = Groupe.groupes[groupesDropdown.value];
                        Groupe.groupes[groupesDropdown.value].vehicles.Add(vehicle);
                    }
                });
                _go.gameObject.SetActive(true);
            }
        }
        else
        {
            groupesDropdown.gameObject.SetActive(false);
            foreach (GameObject curVehicle in Depot.GetVehicles())
            {
                Transform _go = Instantiate(templateDepotVehicle).transform;
                _go.SetParent(listContente);
                _go.Find("Name").GetComponent<Text>().text = curVehicle.GetComponent<VehicleContoler>().vehicleData.name;
                _go.Find("Damage").GetComponent<Text>().text = string.Format("Damage: {0}%", Mathf.Floor(curVehicle.GetComponent<VehicleContoler>().damage * 100));
                _go.Find("Info").GetComponent<Button>().onClick.AddListener(delegate { WindosOpener.OpenVehicleWindow(curVehicle.GetComponent<VehicleContoler>()); });
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
    
}
