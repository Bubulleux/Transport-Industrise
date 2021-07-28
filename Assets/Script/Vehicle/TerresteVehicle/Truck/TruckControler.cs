using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine.Serialization;

namespace Script.Vehicle.TerresteVehicle.Truck
{
	public class TruckControler : TerresteVehicle
	{
		public ProductData productCurTransport;
		public int productQuantity;

		public override float Load()
		{
			LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(VehiclePos);
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
				foreach (ProductData curProduct in vehicleData.productCanTransport)
				{
					if (loadingBay.GetProductions(false).ContainsKey(curProduct) && loadingBay.GetProductions(false)[curProduct].Quantity > 0)
					{
						int productSuccessful = loadingBay.TryToInteract(curProduct, productQuantity - vehicleData.maxProductTransport);
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
