using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupesListWindow : MonoBehaviour
{
    public Transform groupesList;
    public GameObject groupePrefab;
    void Start()
    {
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
            _go.Find("Start").GetComponent<Button>().onClick.AddListener(delegate { curGroupe.StartEveryVehicle(); });
            _go.Find("Stop").GetComponent<Button>().onClick.AddListener(delegate { curGroupe.StopEveryVehicle(); });
            _go.Find("Info").GetComponent<Button>().onClick.AddListener(delegate { WindowsOpener.OpenGroupWindow(curGroupe); });
            _go.gameObject.SetActive(true);
            i++;
        }
    }
}
