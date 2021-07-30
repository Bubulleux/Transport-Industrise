using Script.Mapping;
using Script.Mapping.ParcelType;
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
		
			if (productQuantity != 0 && loadingBay.CanUnload(productCurTransport))
			{
				int productSuccessful = loadingBay.TryToInteract(productCurTransport, productQuantity);
				productQuantity -= productSuccessful;
				timeStop += 2f;
			}
		
			if (productQuantity == 0)
			{
				foreach (ProductData curProduct in vehicleControler.vehicleData.productCanTransport)
				{
					if (Production.GetProduction(loadingBay.GetProductions(), curProduct, false) != null && 
					    Production.GetProduction(loadingBay.GetProductions(), curProduct, false).Quantity > 0)
					{
						int productSuccessful = loadingBay.TryToInteract(curProduct,
							productQuantity - vehicleControler.vehicleData.maxProductTransport);
						productQuantity -= productSuccessful;
						productCurTransport = curProduct;
						timeStop += 2f;
					}
				}
			}
			return timeStop;
		}
	}
}
