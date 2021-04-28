using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class Depot : Parcel
{

    public VehicleContoler BuyVehicle(VehicleData vehicle)
    {
        if (vehicle.price > GameManager.Money)
        {
            return null;
        }
        GameObject _go = Object.Instantiate(Resources.Load("Vehicle") as GameObject);
        _go.transform.position = new Vector3(pos.x, 0f, pos.y);
        _go.GetComponent<VehicleContoler>().vehicleData = vehicle;
        GameManager.Money -= vehicle.price;
        return _go.GetComponent<VehicleContoler>();
    }
    public override void Interact()
    {
        base.Interact();
        WindowsOpener.OpenDepotWindow(this);
    }
}
