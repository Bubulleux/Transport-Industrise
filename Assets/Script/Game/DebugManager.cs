using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DebugManager : MonoBehaviour
{
    /*
     * F1: Get parcel Info
     * F2: Make auto Construct
     * F3: Give Money
     * F4: Interact whith Industrise
     * F5: Create & Serialize Save
     * F6:
     * F7:
     * F8:
     * F9:
     * F10:
     * F11:
     * F12:
     */

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MapManager.map.GetParcel(PlayerControler.GetMoussePos().ToVec2Int()).DebugParcel();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            AutoCreatore.MakeAuto(PlayerControler.GetMoussePos().ToVec2Int());
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameManager.Money += 200000;
        }

        if (Input.GetKeyDown(KeyCode.F4) && MapManager.map.GetparcelType(PlayerControler.GetMoussePos().ToVec2Int()) == typeof(LoadingBay))
        {
            LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(PlayerControler.GetMoussePos().ToVec2Int());
            foreach (KeyValuePair<MaterialData, LoadingBay.MaterialInfo> curMaterial in loadingBay.GetMaterial(true))
            {
                int materialSuccessful = loadingBay.TryToInteract(curMaterial.Key, 20);
                Debug.Log($"Try to give 20 {curMaterial.Key}, material Successful: {materialSuccessful}, now Loading Material: {loadingBay.GetMaterial(true)[curMaterial.Key]}");
            }
            foreach (KeyValuePair<MaterialData, LoadingBay.MaterialInfo> curMaterial in loadingBay.GetMaterial(false))
            {
                int materialSuccessful = -loadingBay.TryToInteract(curMaterial.Key, -20);
                Debug.Log($"Try to take 20 {curMaterial.Key}, material Successful: {materialSuccessful}, now Loading Material: {loadingBay.GetMaterial(false)[curMaterial.Key]}");
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save save = new Save();
            AsyncTask.MonitorTask(save.SaveGame());
        }


    }
}
