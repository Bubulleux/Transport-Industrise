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
        private List<StockBar> stockBars = new List<StockBar>();
        void Start()
        { 
            foreach (var curProduction in loadingBay.productionCumulates)
            {
                Transform gameObject = Instantiate(outputList.GetChild(0), curProduction.isInput ? inputList : outputList);
                
                stockBars.Add(new StockBar()
                {
                    production = curProduction,
                    productionName = gameObject.GetChild(1).GetComponent<Text>(),
                    quantity = gameObject.GetChild(2).GetComponent<Text>(),
                    quantityBar = gameObject.GetChild(0).GetComponent<RectTransform>(),
                });
                gameObject.gameObject.SetActive(true);
            }
        }

        void Update()
        {
            UpdateWindow();
        }
        private void UpdateWindow()
        {
            foreach (var stockBar in stockBars)
            {
                stockBar.productionName.text = stockBar.production.data.productionName;
                stockBar.quantity.text =  $"{stockBar.production.Quantity}/{stockBar.production.MaxQuantity}";
                stockBar.quantityBar.sizeDelta = new Vector2(stockBar.production.Filling * 200, stockBar.quantityBar.sizeDelta.y);
            }
        }
        
        private struct StockBar
        {
            public ProductionCumulate production;
            public Text productionName;
            public Text quantity;
            public RectTransform quantityBar;

        }
    }
}
