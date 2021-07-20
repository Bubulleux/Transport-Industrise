using Script.Vehicle;
using UnityEngine.UI;

namespace Script.UI.Windows
{
    public class GroupWindow : WindowContent
    {
        public Group group;

        public void Start()
        {
            WindowParente.WindowName = "Group: " + group.name;
            UpdateWindow();
        }
        public void UpdateWindow()
        {
            transform.Find("Name").GetComponent<InputField>().text = group.name;
            transform.Find("VehicleCount").GetComponent<Text>().text = "Vehicle Count: " + group.vehicles.Count;
            transform.Find("ForceRoute").GetComponent<Toggle>().isOn = group.forceRoute;
            transform.Find("SetRouteBut").GetComponent<UnityEngine.UI.Button>().interactable = group.forceRoute;
        }

        public void SetRoute()
        {
            WindowsOpener.OpenRouteCreatorWindow(delegate (Route route)
            {
                group.route = route;
                UpdateWindow();
            }, group.route);
        }

        public void SetForceRoute(bool force)
        {
            group.forceRoute = force;
            UpdateWindow();
        }

        public void SetName(string name)
        {
            group.name = name;
        }
    }
}
