using Script.Mapping;
using Script.Mapping.ParcelType;

namespace Script.Vehicle.TerresteVehicle.Truck
{
	public class TruckControler : TerresteVehicle
	{
		public MaterialData materialCurTransport;
		public int materialQuantity;

		public override float Load()
		{
			LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(VehiclePos);
			//Debug.Log("Load Call");
			var timeStop = 0f;
		
			if (materialQuantity != 0 && loadingBay.CanUnload(materialCurTransport))
			{
				int materialSucessful = loadingBay.TryToInteract(materialCurTransport, materialQuantity);
				materialQuantity -= materialSucessful;
				timeStop += 2f;
			}
		
			if (materialQuantity == 0)
			{
				foreach (MaterialData curMaterial in vehicleData.materialCanTransport)
				{
					if (loadingBay.GetMaterial(false).ContainsKey(curMaterial) && loadingBay.GetMaterial(false)[curMaterial].quantity > 0)
					{
						int materialSucessful = loadingBay.TryToInteract(curMaterial, materialQuantity - vehicleData.maxMaterialTransport);
						materialQuantity -= materialSucessful;
						materialCurTransport = curMaterial;
						timeStop += 2f;
					}
				}
			}
			return timeStop;
		}
	}
}
