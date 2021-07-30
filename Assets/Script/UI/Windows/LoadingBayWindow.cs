using System.Collections.Generic;
using Script.Mapping.ParcelType;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Windows
{
    public class LoadingBayWindow : WindowContent
    {
        public Transform inputList, outputList;
        public LoadingBay loadingBay;
        void Start()
        {
            UpdateWindow();
        }

        void Update()
        {
            UpdateWindow();
        }
        private void UpdateWindow()
        {
            foreach(Transform curInput in inputList)
            {
                if (curInput.gameObject.activeSelf == true)
                {
                    Destroy(curInput.gameObject);
                }
            }
            foreach(Transform curOutput in outputList)
            {
                if (curOutput.gameObject.activeSelf == true)
                {
                    Destroy(curOutput.gameObject);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                foreach (var curProduction in loadingBay.GetProductions(i != 0))
                {
                    Transform _go = Instantiate(outputList.GetChild(0));
                    _go.SetParent(i == 0 ? outputList : inputList);
                    _go.GetChild(1).GetComponent<Text>().text = curProduction.data.name;
                    _go.GetChild(2).GetComponent<Text>().text =  $"{curProduction.Quantity}/{curProduction.maxQuantity}";
                    _go.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(curProduction.Filling * 200, _go.Find("ProgressBar").GetComponent<RectTransform>().sizeDelta.y);
                    _go.gameObject.SetActive(true);
                }
            }
        
        }
    }
}
