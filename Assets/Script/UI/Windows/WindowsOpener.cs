using Script.Mapping.ParcelType;
using Script.Vehicle;
using UnityEngine;

namespace Script.UI.Windows
{
    public static class WindowsOpener 
    {
        public static GameObject OpenDepotWindow(Depot depot)
        {
            Window window = OpenWindowByName("Depot");
            window.Contente.GetComponent<DepotWindow>().depot = depot;
            return window.gameObject;
        }

        public static GameObject OpenRouteListWindow()
        {
            Window window = OpenWindowByName("RoutesList");
            return window.gameObject;
        }
        public static GameObject OpenGroupesListWindow()
        {
            return OpenWindowByName("GroupsList").gameObject;
        }

        public static GameObject OpenRouteCreatorWindow(RouteCreatorWindow.FunctionFinish _functionFinish, Route _route = null)
        {
            Window window = OpenWindowByName("RoutesCreator");
            window.Contente.GetComponent<RouteCreatorWindow>().functionFinish = _functionFinish;
            if (_route != null)
            {
                window.GetComponent<Window>().Contente.GetComponent<RouteCreatorWindow>().route = _route;
            }
            return window.gameObject;
        }

        public static GameObject OpenVehicleWindow(VehicleContoler vehicle)
        {
            Window window = OpenWindowByName("Vehicle");
            window.Contente.GetComponent<VehicleWIndow>().vehicle = vehicle;
            return window.gameObject;
        }

        public static GameObject OpenGroupWindow(Group group)
        {
            Window window = OpenWindowByName("Group");
            window.Contente.GetComponent<GroupWindow>().group = group;
            return window.gameObject;
        }

        public static GameObject OpenLoadingBay(LoadingBay loadingBay)
        {
            Window window = OpenWindowByName("LoadingBay");
            window.Contente.GetComponent<LoadingBayWindow>().loadingBay = loadingBay;
            return window.gameObject;
        }

        private static Window OpenWindowByName(string name)
        {
            GameObject _window = Object.Instantiate(Resources.Load("UI/Window", typeof(GameObject)) as GameObject);
            GameObject _windowContente = Object.Instantiate(Resources.Load("UI/WindowContent/" + name, typeof(GameObject)) as GameObject);

            _window.transform.SetParent(GameObject.Find("Canvas").transform, false);
            _windowContente.transform.SetParent(_window.transform);

            _windowContente.name = "WindowContent";

            RectTransform windowRectTransform = _window.GetComponent<RectTransform>();
            RectTransform windowContenteRectTransform = _windowContente.GetComponent<RectTransform>();

            Vector2 _windowContenteSize = windowContenteRectTransform.sizeDelta;
            windowRectTransform.sizeDelta = new Vector2(0, 20) + _windowContenteSize;

            windowContenteRectTransform.anchorMin = Vector2.zero;
            windowContenteRectTransform.anchorMax = Vector2.one;
            windowContenteRectTransform.pivot = Vector2.zero;

            windowContenteRectTransform.offsetMin = Vector2.zero;
            windowContenteRectTransform.offsetMax = new Vector2(0, -20);

            _window.transform.localPosition = Vector3.zero;
            return _window.GetComponent<Window>();
        }
    }
}
