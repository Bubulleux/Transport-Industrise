using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            foreach (KeyValuePair<MaterialData, LoadingBay.MaterialInfo> curMaterial in loadingBay.GetMaterial(i != 0))
            {
                Transform _go = Instantiate(outputList.GetChild(0));
                _go.SetParent(i == 0 ? outputList : inputList);
                _go.GetChild(1).GetComponent<Text>().text = curMaterial.Key.name;
                _go.GetChild(2).GetComponent<Text>().text =  $"{curMaterial.Value.quantity}/{curMaterial.Value.maxQuantity}";
                _go.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(curMaterial.Value.Filling * 200, _go.Find("ProgressBar").GetComponent<RectTransform>().sizeDelta.y);
                _go.gameObject.SetActive(true);
            }
        }
        
    }
}
