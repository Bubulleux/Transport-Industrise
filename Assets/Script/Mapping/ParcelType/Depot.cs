using System;
using Newtonsoft.Json;
using Script.Game;
using Script.UI.Windows;
using Script.Vehicle;
using Script.Vehicle.TerresteVehicle;
using Script.Vehicle.TerresteVehicle.Truck;
using Script.Vehicle.VehicleData;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Depot : Road
    {
        public override void InitializationSecondary()
        {
            color = Color.red;
            prefab = Resources.Load<GameObject>("ParcelGFX/Depot");
        }

        public override void UpdateRoadObject(bool debug = false)  {  } 
        
        public VehicleContoler BuyVehicle(VehicleData vehicle)
        {
            if (vehicle.price > GameManager.Money)
            {
                return null;
            }
            GameObject _go = UnityEngine.Object.Instantiate(Resources.Load("Vehicle") as GameObject);
            _go.transform.position = new Vector3(pos.x, 0f, pos.y);
            
            _go.GetComponent<VehicleContoler>().vehicleData = vehicle;
            _go.AddComponent(vehicle.ProductIsPeople ? typeof(BusLoader) : typeof(TruckLoader));
            _go.GetComponent<VehicleLoader>().vehicleControler = _go.GetComponent<TerresteVehicle>();
            GameManager.Money -= vehicle.price;
            return _go.GetComponent<VehicleContoler>();
        }
        public override void Interact()
        {
            WindowsOpener.OpenDepotWindow(this);
        }

        public override bool CanConnect(Vector2Int connectionPos)
        {
            return Array.IndexOf(MapManager.parcelAround, connectionPos - pos) == (int)orientation;
        }
    }
}
