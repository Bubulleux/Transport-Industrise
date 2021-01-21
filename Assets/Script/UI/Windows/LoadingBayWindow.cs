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
            Dictionary<MaterialData, int> materialQuantity = i == 0 ? loadingBay.GetMaterialOutpute() : loadingBay.GetMaterialInput();
            foreach (KeyValuePair<MaterialData, float> curMaterial in loadingBay.GetMaterialRatio(i != 0))
            {
                Transform _go = Instantiate(outputList.Find("Template"));
                _go.SetParent(i == 0 ? outputList : inputList);
                _go.Find("Material").GetComponent<Text>().text = curMaterial.Key.ToString();
                _go.Find("Quantity").GetComponent<Text>().text = materialQuantity[curMaterial.Key].ToString();
                _go.Find("ProgressBar").GetComponent<RectTransform>().sizeDelta = new Vector2(curMaterial.Value * 200, _go.Find("ProgressBar").GetComponent<RectTransform>().sizeDelta.y);
                _go.gameObject.SetActive(true);
            }
        }
        
    }
}
