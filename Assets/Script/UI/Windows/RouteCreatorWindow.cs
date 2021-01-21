using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteCreatorWindow : WindowContent
{

    public delegate void FunctionFinish(Route route);
    public FunctionFinish functionFinish;
    public Route route;
    public Transform pointsList;
    public GameObject pointPrefab;

    private void Start()
    {
        if (route == null)
        {
            route = new Route();
        }
        WindowParente.WindowName = "Route Creator";
        route.name = "Name Random" + Random.Range(0, 100);
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
            _go.Find("Delete").GetComponent<Button>().onClick.AddListener(delegate { Delete(_i); });
            _go.Find("UpArrow").GetComponent<Button>().onClick.AddListener(delegate { Move(_i, -1); });
            _go.Find("DnArrow").GetComponent<Button>().onClick.AddListener(delegate { Move(_i, 1); });
            _go.gameObject.SetActive(true);
            i++;
        }
    }
    public void AddPoint()
    {
        PlayerControler.instance.RedirectTool(delegate (Vector2Int pos)
        {
            if (this == null)
            {
                PlayerControler.instance.toolRedirection = null;
                return;
            }
            System.Type type = MapManager.map.parcels[pos.x, pos.y].GetType();
            if (type == typeof(Depot) || type == typeof(LoadingBay))
            {
                route.points.Add(pos);
                UpdateList();
            }
        });
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
        functionFinish(route);
        Close();
    }
}

public class Route
{
    public string name;
    public List<Vector2Int> points = new List<Vector2Int>();
}