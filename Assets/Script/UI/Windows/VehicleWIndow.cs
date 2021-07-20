using System.Collections.Generic;
using Script.Vehicle;
using UnityEngine.UI;

namespace Script.UI.Windows
{
    public class VehicleWIndow : WindowContent
    {
        public VehicleContoler vehicle;

        private void Start()
        {
            UpdateWindow();
        }

        public void UpdateWindow()
        {
            transform.Find("Name").GetComponent<Text>().text = vehicle.vehicleData.name;
            transform.Find("Damage").GetComponent<Text>().text = "Damage:" + vehicle.damage.ToString();
            transform.Find("ID").GetComponent<Text>().text = "ID: " + vehicle.Id;
            transform.Find("Route").GetComponent<UnityEngine.UI.Button>().interactable = (vehicle.MyGroup == null || vehicle.MyGroup.forceRoute == false);
            Dropdown group = transform.Find("Group").GetComponent<Dropdown>();
            List<string> options = new List<string>();
            int index = Group.groups.Count;
            foreach(Group curGroup in Group.groups)
            {
                options.Add(curGroup.name);
                if (vehicle.MyGroup == curGroup)
                {
                    index = options.Count - 1;
                }
            }
            options.Add("None");
            group.ClearOptions();
            group.AddOptions(options);
            group.SetValueWithoutNotify(index);
        }

        public void GroupSet(int index)
        {
            if (index == Group.groups.Count)
            {
                vehicle.MyGroup = null;
            }
            else
            {
                vehicle.MyGroup = Group.groups[index];
            }
            UpdateWindow();
        }

        public void SetRoute()
        {
            WindowsOpener.OpenRouteCreatorWindow(delegate (Route route) { vehicle.MyRoute = route; }, vehicle.MyRoute);
        }
    }
}
