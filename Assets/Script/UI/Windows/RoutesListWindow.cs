using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Windows
{
    public class RoutesListWindow : MonoBehaviour
    {
        public static List<Route> routes = new List<Route>();
        public Transform routesList;
        public GameObject routePrefab;
        void Start()
        {
            UpdateList();
        }

        public void ButCreateRoute()
        {
            WindowsOpener.OpenRouteCreatorWindow(delegate (Route route) { routes.Add(route); PrintList(); UpdateList(); });
        }
        public void UpdateList()
        {
            foreach (Transform curChild in routesList)
            {
                if (curChild.gameObject.activeSelf)
                {
                    Destroy(curChild.gameObject);
                }
            }
            int i = 0;
            foreach (Route curRoute in routes)
            {
                Transform _go = Instantiate(routePrefab).transform;
                _go.SetParent(routesList);
                _go.Find("Name").GetComponent<Text>().text = curRoute.name;
                int _i = i;
                _go.Find("Edit").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { WindowsOpener.OpenRouteCreatorWindow(delegate (Route route) { routes[_i] = route; UpdateList(); }, curRoute); });
                _go.Find("Delete").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Debug.Log(_i);  routes.RemoveAt(_i); UpdateList(); });
                _go.gameObject.SetActive(true);
                i++;
            }
        }
        public void PrintList()
        {
            foreach(Route curRoute in routes)
            {
                Debug.Log(curRoute.name);
            }
        }
    }
}
