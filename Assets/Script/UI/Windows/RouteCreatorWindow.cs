using System.Collections;
using System.Collections.Generic;
using Script.Controler;
using Script.Mapping;
using Script.Mapping.ParcelType;
using Script.UI.Windows;
using UnityEngine;
using UnityEngine.UI;


public class RouteCreatorWindow : WindowContent
{

    public delegate void FunctionFinish(Route route);
    public FunctionFinish functionFinish;
    public Route route;
    public Transform pointsList;
    public GameObject pointPrefab;
    private RouteConfigurator tool;

    private void Start()
    {
        if (route == null)
        {
            route = new Route();
        }
        WindowParente.WindowName = "Route Creator";
        route.name = "Name Random" + Random.Range(0, 100);
        tool = new RouteConfigurator(route, this);
        UpdateList();

    }
    public void UpdateList()
    {
        foreach(Transform curChild in pointsList)
        {
            if (curChild.gameObject.activeSelf)
            {
                Destroy(curChild.gameObject);
            }
        }
        int i = 0;
        foreach(Vector2Int curPoint in route.points)
        {
            Transform _go = Instantiate(pointPrefab).transform;
            _go.SetParent(pointsList);
            int _i = i;
            _go.Find("Name").GetComponent<Text>().text = curPoint.ToString();
            _go.Find("Number").GetComponent<Text>().text = _i + "-";
            _go.Find("Delete").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Delete(_i); });
            _go.Find("UpArrow").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Move(_i, -1); });
            _go.Find("DnArrow").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Move(_i, 1); });
            _go.gameObject.SetActive(true);
            i++;
        }
    }
    public void AddPoint()
    {
        if (PlayerControler.instance.CurTool != tool)
            PlayerControler.instance.CurTool = tool;
        else
            PlayerControler.instance.CurTool = null;
    }
    public void Delete(int index)
    {
        Debug.Log(index);
        route.points.RemoveAt(index);
        UpdateList();
    }
    public void Move(int index, int dir)
    {
        if (index + dir > route.points.Count || index + dir < 0)
        {
            return;
        }
        Vector2Int value = route.points[index];
        route.points.RemoveAt(index);
        route.points.Insert(index + dir, value);
        UpdateList();
    }
    public void Finish()
    {
        PlayerControler.instance.CurTool = null;
        functionFinish(route);
        Close();
    }
}

public class Route
{
    public string name;
    public List<Vector2Int> points = new List<Vector2Int>();
}
