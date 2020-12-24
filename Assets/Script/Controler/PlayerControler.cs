using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /*
     * F1: Get parcel Info
     * F2: Make auto Construct
     * F3: Print vehicle info
     * F4: Interact whith Industrise
     * F5: Create & Serialize Save
     * F6: LoadSave
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toolRedirection = null;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log(GetMoussePos().ToVec2Int());
            string parcelJson = Save.GetJson(MapManager.map.GetParcel(GetMoussePos().ToVec2Int()));
            Debug.Log(parcelJson);
            Debug.Log(Save.GetObject<Parcel>(parcelJson).GetType());
            if (MapManager.map.GetparcelType(GetMoussePos().ToVec2Int()) == typeof(LoadingBay))
            {
                string result = "Inpute:";
                foreach(KeyValuePair<Materials, int> curMaterial in MapManager.map.GetParcel<LoadingBay>(GetMoussePos().ToVec2Int()).GetMaterialInput())
                {
                    result += $"\n{curMaterial.Key}: {curMaterial.Value}";
                }
                result += "\nOutpute: ";
                foreach (KeyValuePair<Materials, int> curMaterial in MapManager.map.GetParcel<LoadingBay>(GetMoussePos().ToVec2Int()).GetMaterialOutpute())
                {
                    result += $"\n{curMaterial.Key}: {curMaterial.Value}";
                }
                Debug.Log(result);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AutoCreatore.MakeAuto(GetMoussePos().ToVec2Int());
        }

        if (Input.GetKeyDown(KeyCode.F4) && MapManager.map.GetparcelType(GetMoussePos().ToVec2Int()) == typeof(LoadingBay))
        {
            LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(GetMoussePos().ToVec2Int());
            foreach(KeyValuePair<Materials, int> curMaterial in loadingBay.GetMaterialInput())
            {
                int materialSuccessful = loadingBay.TryToInteract(curMaterial.Key, 20);
                Debug.Log($"Try to give 20 {curMaterial.Key}, material Successful: {materialSuccessful}, now Loading Material: {loadingBay.GetMaterialInput()[curMaterial.Key]}");
            }
            foreach (KeyValuePair<Materials, int> curMaterial in loadingBay.GetMaterialOutpute())
            {
                int materialSuccessful = -loadingBay.TryToInteract(curMaterial.Key, -20);
                Debug.Log($"Try to take 20 {curMaterial.Key}, material Successful: {materialSuccessful}, now Loading Material: {loadingBay.GetMaterialOutpute()[curMaterial.Key]}");
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save save = new Save();
            save.SerializeAndSave();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            Save save = new Save();
            save.LoadAndDeserialize();
        }

        if (Input.GetMouseButtonDown(0) && GetMoussePos() != Vector3.zero)
        {
            if (toolRedirection == null)
            {
                switch (curTool)
                {
                    case Tools.none:
                        object construction = MapManager.map.parcels[GetMoussePos().ToVec2Int().x, GetMoussePos().ToVec2Int().y];
                        if (construction != null)
                        {
                            if (construction is Depot)
                            {
                                WindowsOpener.OpenDepotWindow(MapManager.map.GetParcel<Depot>(GetMoussePos().ToVec2Int()));
                            }
                        }
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
                }
            }
            else
            {
                toolRedirection(GetMoussePos().ToVec2Int());
            }
            
        }
    }

    public Vector3 GetMoussePos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
                lastPos += new Vector2Int(lastPos.x < endMouse.x ? 1 : -1, 0);
                MapManager.map.AddRoad(lastPos);
            }
            else
            {
                lastPos += new Vector2Int(0, lastPos.y < endMouse.y ? 1 : -1);
                MapManager.map.AddRoad(lastPos);
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
        loadingBay
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
