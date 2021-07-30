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

        private static Dictionary<string, int> counters = new Dictionary<string, int>();
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
                foreach (var curProduction in loadingBay.GetProductions(true))
                {
                    int productSuccessful = loadingBay.TryToInteract(curProduction.data, 20);
                    Debug.Log($"Try to give 20 {curProduction.data}, product Successful: {productSuccessful}");
                }
                foreach (var curProduction in loadingBay.GetProductions(false))
                {
                    int productSuccessful = -loadingBay.TryToInteract(curProduction.data, -20);
                    Debug.Log($"Try to take 20 {curProduction.data}, product Successful: {productSuccessful}");
                }
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                MapManager.Selector.SelectParcel(PlayerControler.GetMoussePos().ToVec2Int(),
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

        public static int Count(string counterName, int value = 1)
        {
            if (!counters.ContainsKey(counterName))
                counters.Add(counterName, 0);
            counters[counterName] += value;
            return counters[counterName];
        }

        public static void GetCounter(string counterName, bool clear = true)
        {
            Debug.Log($"Counter: {counterName}, Value: {counters[counterName]}");
            counters.Remove(counterName);
        }
    }
}
