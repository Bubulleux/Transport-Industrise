using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControler : MonoBehaviour
{
    public Camera cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(MakeRoad());
        }
    }

    public Vector3 GetMoussePos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public static Vector2Int Vec3ToVec2Int(Vector3 vec)
    {
        return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
    }

    public IEnumerator MakeRoad()
    {
        Vector2Int startMouse = Vec3ToVec2Int(GetMoussePos());
        while (Input.GetMouseButton(0))
        {
            yield return new WaitForFixedUpdate();
        }
        Vector2Int endMouse = Vec3ToVec2Int(GetMoussePos());
        Vector2Int lastPos = startMouse;
        Map.instence.AddRoad(startMouse);
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
                Map.instence.AddRoad(lastPos);
            }
            else
            {
                lastPos = lastPos + new Vector2Int(0, lastPos.y < endMouse.y ? 1 : -1);
                Map.instence.AddRoad(lastPos);
            }
           
        }
    }
}
