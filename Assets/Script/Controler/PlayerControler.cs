using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler instance;
    public Camera cam;
    public Tools curTool;
    public delegate void MainToolRedir(Vector2Int posMouse);
    public MainToolRedir toolRedirection = null;

    private void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toolRedirection = null;
        }
        if (Input.GetMouseButtonDown(0) && GetMoussePos() != Vector3.zero)
        {
            if (toolRedirection == null)
            {
                switch (curTool)
                {
                    case Tools.none:
                        MapManager.map.GetParcel(GetMoussePos().ToVec2Int()).Interact();
                        break;
                    case Tools.road:
                        StartCoroutine(MakeRoad());
                        break;
                    case Tools.depot:
                        MapManager.map.AddConstruction(GetMoussePos().ToVec2Int(), new Depot());
                        break;
                    case Tools.loadingBay:
                        MapManager.map.AddConstruction(GetMoussePos().ToVec2Int(), new LoadingBay());
                        break;
                    case Tools.destroy:
                        MapManager.map.Destroy(GetMoussePos().ToVec2Int());
                        break;
                }
            }
            else
            {
                toolRedirection(GetMoussePos().ToVec2Int());
            }
            
        }

        
    }

    public static Vector3 GetMoussePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && !PointerIsOverUI())
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
        while (true)
        {
            if (lastPos == endMouse)
            {
                break;
            }
            float xProgress = Mathf.Abs(lastPos.x - startMouse.x) / (float)Mathf.Abs(startMouse.x - endMouse.x);
            float yProgress = Mathf.Abs(lastPos.y - startMouse.y) / (float)Mathf.Abs(startMouse.y - endMouse.y);
            xProgress = (float.IsNaN(xProgress) || float.IsInfinity(xProgress)) ? 1 : xProgress;
            yProgress = (float.IsNaN(yProgress) || float.IsInfinity(yProgress)) ? 1 : yProgress;
            if (xProgress < yProgress)
            {
                for (int i = 0; i < 50; i++)
                {
                    lastPos += new Vector2Int(lastPos.x < endMouse.x ? 1 : -1, 0);
                    MapManager.map.AddRoad(lastPos);
                    if (lastPos.x == endMouse.x) { break; }
                }
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    lastPos += new Vector2Int(0, lastPos.y < endMouse.y ? 1 : -1);
                    MapManager.map.AddRoad(lastPos);
                    if (lastPos.y == endMouse.y) { break; }
                }
            }
           
        }
    }

    public void SetTool(int tool)
    {
        curTool = (Tools)tool;
        toolRedirection = null;
    }

    public void RedirectTool( MainToolRedir func)
    {
        toolRedirection = func;
        Debug.Log("Redirec Main Tool");
    }
    public enum Tools
    {
        none,
        road,
        depot,
        loadingBay,
        destroy
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
