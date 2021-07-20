using Script.Vehicle;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Windows
{
    public class GroupesListWindow : WindowContent
    {
        public Transform groupesList;
        public GameObject groupePrefab;
        void Start()
        {
            WindowParente.WindowName = "Groups List";
            UpdateList();
        }

        public void ButCreateGroupe()
        {
            new Group();
            UpdateList();
        }
        public void UpdateList()
        {
            foreach (Transform curChild in groupesList)
            {
                if (curChild.gameObject.activeSelf)
                {
                    Destroy(curChild.gameObject);
                }
            }
            int i = 0;
            foreach (Group curGroupe in Group.groups)
            {
                Transform _go = Instantiate(groupePrefab).transform;
                _go.SetParent(groupesList);
                _go.Find("Name").GetComponent<Text>().text = curGroupe.name;
                int _i = i;
                _go.Find("Start").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { curGroupe.StartEveryVehicle(); });
                _go.Find("Stop").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { curGroupe.StopEveryVehicle(); });
                _go.Find("Info").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { WindowsOpener.OpenGroupWindow(curGroupe); });
                _go.gameObject.SetActive(true);
                i++;
            }
        }
    }
}
