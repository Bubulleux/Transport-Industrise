using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepotWindow : MonoBehaviour
{
    public GameObject template;
    public Transform listContente;
    private Vector2Int depotpos;
    public Depot Depot { set => Map.instence.parcels[depotpos.x, depotpos.y].construction = value;  get => (Depot)Map.instence.parcels[depotpos.x, depotpos.y].construction;}
    public bool onStore = false;
    public Text butTxt;
    public void Init(Vector2Int _depot)
    {
        depotpos = _depot;
        UpdateWindow();
    }
    public void UpdateWindow()
    {
        foreach(GameObject curVehicle in Depot.vehicles)
        {
            Transform _go = Instantiate(template).transform;
            transform.SetParent(listContente);
            _go.Find("Name").GetComponent<Text>().text = curVehicle.GetComponent<VehicleContoler>().vehicleData.name;
            _go.Find("Damage").GetComponent<Text>().text = string.Format("Damage: {0}%",Mathf.Floor(curVehicle.GetComponent<VehicleContoler>().damage * 100));
            _go.Find("Info").GetComponent<Button>().onClick.AddListener(delegate{ Debug.Log("Buton Clk"); });
            _go.gameObject.SetActive(true);
        }
        template.SetActive(false);
    }
    public void Store()
    {
        onStore = !onStore;
        butTxt.text = onStore ? "Depot" : "Store";
        UpdateWindow();
    }
}
