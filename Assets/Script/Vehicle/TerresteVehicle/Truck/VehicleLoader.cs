using UnityEngine;

namespace Script.Vehicle.TerresteVehicle.Truck
{
	public class VehicleLoader : MonoBehaviour
	{
		public TerresteVehicle vehicleControler;
		
		public virtual float Load()
		{
			return 0f;
		}
	}
}