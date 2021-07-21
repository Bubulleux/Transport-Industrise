using System.Collections.Generic;
using Script.Controler;
using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

namespace Script.Game
{
    public class DebugManager : MonoBehaviour
    {
        /*
     * F1: Get parcel Info
     * F2: Make auto Construct
     * F3: Give Money
     * F4: Interact Debug
     * F5: 
     * F6: Select Parcel
     * F7: Unselect parcel
     * F8: Clear Selection
     * F9: Select Area
     * F10:
     * F11:
     * F12:
     */
        
        private Vector2Int _startSelection;
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

            if (Input.GetKeyDown(KeyCode.F4))
            {
                System.Type parcelType = MapManager.map.GetparcelType(PlayerControler.GetMoussePos().ToVec2Int());
                if (parcelType != typeof(LoadingBay) && parcelType != typeof(BusStop))
                {
                    return;
                }
            
                LoadingBay loadingBay = MapManager.map.GetParcel<LoadingBay>(PlayerControler.GetMoussePos().ToVec2Int());
                foreach (KeyValuePair<MaterialData, LoadingBay.MaterialInfo> curMaterial in loadingBay.GetMaterial(true))
                {
                    int materialSuccessful = loadingBay.TryToInteract(curMaterial.Key, 20);
                    Debug.Log($"Try to give 20 {curMaterial.Key}, material Successful: {materialSuccessful}, now Loading Material: {loadingBay.GetMaterial(true)[curMaterial.Key].quantity}");
                }
                foreach (KeyValuePair<MaterialData, LoadingBay.MaterialInfo> curMaterial in loadingBay.GetMaterial(false))
                {
                    int materialSuccessful = -loadingBay.TryToInteract(curMaterial.Key, -20);
                    Debug.Log($"Try to take 20 {curMaterial.Key}, material Successful: {materialSuccessful}, now Loading Material: {loadingBay.GetMaterial(false)[curMaterial.Key].quantity}");
                }
            }
            // if (Input.GetKeyDown(KeyCode.F4) && MapManager.map.GetparcelType(PlayerControler.GetMoussePos().ToVec2Int()) == typeof(BusStop))
            // {
            //     BusStop busStop = MapManager.map.GetParcel<BusStop>(PlayerControler.GetMoussePos().ToVec2Int());
            //     int materialSuccessful = busStop.TryToInteract(BusStop.PeopleMatarial, -20);
            //     Debug.Log($"{materialSuccessful}, now Loading Material: {busStop.GetMaterial(false)[BusStop.PeopleMatarial].quantity}");
            //
            // }

            // if (Input.GetKeyDown(KeyCode.F5))
            // {
            //     Save.Save save = new Save.Save();
            //     AsyncTask.MonitorTask(save.SaveGame());
            // }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                MapManager.Selector.SelectionParcel(PlayerControler.GetMoussePos().ToVec2Int(),
                    new Color(Random.value, Random.value, Random.value));
            }
            
            if (Input.GetKeyDown(KeyCode.F7))
            {
                MapManager.Selector.UnSelectParcel(PlayerControler.GetMoussePos().ToVec2Int());
            }
            
            if (Input.GetKeyDown(KeyCode.F8))
            {
                MapManager.Selector.ClearSelection();
            }
            
            if (Input.GetKeyDown(KeyCode.F9))
            {
                _startSelection = PlayerControler.GetMoussePos().ToVec2Int();
            }

            if (Input.GetKey(KeyCode.F9))
            {
                MapManager.Selector.UnSelectColor(Color.green);
                MapManager.Selector.SelectArea(_startSelection, PlayerControler.GetMoussePos().ToVec2Int(), Color.green);
            }
            if (Input.GetKeyUp(KeyCode.F9))
            {
                MapManager.Selector.UnSelectColor(Color.green);
                MapManager.Selector.SelectArea(_startSelection, PlayerControler.GetMoussePos().ToVec2Int(), Color.red);
            }


        }
    }
}
