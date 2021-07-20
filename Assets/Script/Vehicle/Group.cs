using System.Collections.Generic;
using Newtonsoft.Json;
using Script.UI.Windows;

namespace Script.Vehicle
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Group
    {
        [JsonIgnore]
        public static List<Group> groups = new List<Group>();
        public string name;
        [JsonIgnore]
        public List<VehicleContoler> vehicles = new List<VehicleContoler>();

        public bool forceRoute;
        public Route route;

        public Group()
        {
            groups.Add(this);
            name = "Random Groupe";
        }


        public void StartEveryVehicle()
        {
            foreach(VehicleContoler curVehicle in vehicles)
            {
                curVehicle.StartVehicle();
            }
        }

        public void StopEveryVehicle()
        {
            foreach (VehicleContoler curVehicle in vehicles)
            {
                curVehicle.ReturnInDepot();
            }
        }
    }
}
