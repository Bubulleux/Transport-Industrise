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
    public Depot Depot { set => MapManager.map.parcels[depotpos.x, depotpos.y].construction = value;  get => (Depot)MapManager.map.parcels[depotpos.x, depotpos.y].construction;}
    public bool onStore = false;
    public Text butTxt;
    public VehicleData[] vehicleInStore;
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
            foreach (VehicleData curVehicle in vehicleInStore)
            {
                Transform _go = Instantiate(templateStoreVehicle).transform;
                _go.SetParent(listContente);
                _go.Find("Name").GetComponent<Text>().text = curVehicle.name;
                _go.Find("Description").GetComponent<Text>().text = curVehicle.description;
                _go.Find("Buy").GetComponent<Button>().onClick.AddListener(delegate { Depot.BuyVehicle(curVehicle); });
                _go.gameObject.SetActive(true);
            }
        }
        else
        {

            foreach (GameObject curVehicle in Depot.GetVehicles())
            {
                Transform _go = Instantiate(templateDepotVehicle).transform;
                _go.SetParent(listContente);
                _go.Find("Name").GetComponent<Text>().text = curVehicle.GetComponent<VehicleContoler>().vehicleData.name;
                _go.Find("Damage").GetComponent<Text>().text = string.Format("Damage: {0}%", Mathf.Floor(curVehicle.GetComponent<VehicleContoler>().damage * 100));
                _go.Find("Info").GetComponent<Button>().onClick.AddListener(delegate { Debug.Log("Buton Clk"); });
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
