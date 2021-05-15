using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class TruckControler : TerresteVehicle
{
	public MaterialData materialCurTransport;
	public int materialQuantity;

	public override float Load()
	{
		LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(VehiclePos);

		if (materialQuantity != 0 && loadingBay.CanUnload(materialCurTransport))
		{
			int materialSucessful = loadingBay.TryToInteract(materialCurTransport, materialQuantity);
			materialQuantity -= materialSucessful;
			return 2f;
		}
		else
		{
			foreach (MaterialData curMaterial in vehicleData.materialCanTransport)
			{
				if (loadingBay.GetMaterial(false).ContainsKey(curMaterial) && loadingBay.GetMaterial(false)[curMaterial].quantity > 0)
				{
					int materialSucessful = loadingBay.TryToInteract(curMaterial, materialQuantity - vehicleData.maxMaterialTransport);
					materialQuantity -= materialSucessful;
					materialCurTransport = curMaterial;
					return 2f;
				}
			}
		}
		return 0f;
	}
}
