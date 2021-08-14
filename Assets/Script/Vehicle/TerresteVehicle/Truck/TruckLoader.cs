using Script.Game;
using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Vehicle.TerresteVehicle.Truck
{
	public class TruckLoader : VehicleLoader
	{
		public ProductData productCurTransport;
		public int productQuantity;

		public override float Load()
		{
			LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(vehicleControler.VehiclePos);
			//Debug.Log("Load Call");
			var timeStop = 0f;
		
			if (productQuantity != 0)
			{
				int productSuccessful = loadingBay.TryToInteract(productCurTransport, productQuantity);
				Debug.Log($"Unload {productCurTransport.name} Successful: {productSuccessful}, Try: {productQuantity}");
				productQuantity -= productSuccessful;
				if (productSuccessful != 0)
				{
					timeStop += 2f;
					Debug.Log("Unload");
				}
			}
		
			if (productQuantity == 0)
			{
				foreach (ProductData curProduct in vehicleControler.vehicleData.productCanTransport)
				{
					if (Production.GetProduction(loadingBay.GetProductions(), curProduct, false) != null && 
					    Production.GetProduction(loadingBay.GetProductions(), curProduct, false).Quantity > 0 &&
					    (productQuantity == 0 || productCurTransport == curProduct))
					{
						int productSuccessful = loadingBay.TryToInteract(curProduct,
							productQuantity - vehicleControler.vehicleData.maxProductTransport);
						productQuantity -= productSuccessful;
						productCurTransport = curProduct;
						timeStop += 2f;
						break;
					}
				}
			}
			return timeStop;
		}
	}
}
