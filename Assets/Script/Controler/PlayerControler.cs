using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControler : MonoBehaviour
{
    public Camera cam;
    public Tools curTool;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetMoussePos() != Vector3.zero)
        {
            switch(curTool)
            {
                case Tools.none:
                    object construction = MapManager.map.parcels[GetMoussePos().ToVec2Int().x, GetMoussePos().ToVec2Int().y].construction;
                    if (construction != null)
                    {
                        if (construction is Depot)
                        {
                            WindosOpener.openDepotWindow(GetMoussePos().ToVec2Int());
                        }
                    }
                    break;
                case Tools.road:
                    StartCoroutine(MakeRoad());
                    break;
                case Tools.depot:
                    MapManager.map.AddDepot(GetMoussePos().ToVec2Int());
                    break;
            }
        }
    }

    public Vector3 GetMoussePos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !PointerIsOverUI())
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public IEnumerator MakeRoad()
    {
        Vector2Int startMouse = GetMoussePos().ToVec2Int();
        while (Input.GetMouseButton(0))
        {
            yield return new WaitForFixedUpdate();
        }
        Vector2Int endMouse = GetMoussePos().ToVec2Int();
        Vector2Int lastPos = startMouse;
        MapManager.map.AddRoad(startMouse);
        int i = 0;
        while (true)
        {
            i++;
            if (lastPos == endMouse || i > 1000)
            {
                break;
            }
            if (Mathf.Abs(lastPos.x - endMouse.x) / (float)Mathf.Abs(startMouse.x - endMouse.x) > Mathf.Abs(lastPos.y - endMouse.y) / (float)Mathf.Abs(startMouse.y - endMouse.y))
            {
                lastPos = lastPos + new Vector2Int(lastPos.x < endMouse.x ? 1 : -1, 0);
                MapManager.map.AddRoad(lastPos);
            }
            else
            {
                lastPos = lastPos + new Vector2Int(0, lastPos.y < endMouse.y ? 1 : -1);
                MapManager.map.AddRoad(lastPos);
            }
           
        }
    }

    public void SetTool(int tool)
    {
        curTool = (Tools)tool;
    }
    public enum Tools
    {
        none,
        road,
        depot
    }

    public static bool PointerIsOverUI()
    {
        foreach (GameObject curWindow in GameObject.FindGameObjectsWithTag("Window"))
        {
            if (curWindow.GetComponent<Window>().pointerOverMe)
            {
                return true;
            }
        }
        return false;
    }
}
