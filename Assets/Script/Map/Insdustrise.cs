 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class Industrise
{
    public IndustriseData industriseData;
    public Dictionary<MaterialData, int> materialsInpute = new Dictionary<MaterialData, int>();
    public Dictionary<MaterialData, int> materialsOutpute = new Dictionary<MaterialData, int>();
    public Vector2Int MasterPos;
    public static int maxMaterialCanStock = 300;
    public float materialProductionRatio;

    public float productionCoolDown;

    public Industrise(Vector2Int _pos, Map _mapData)
    {
        MasterPos = _pos;
        IndustriseData[] allIndustriseData = FIleSys.GetAllInstances<IndustriseData>();
        industriseData = allIndustriseData[Random.Range(0, allIndustriseData.Length)];
        for (int y = -1; y <= 2; y++)
        {
            for (int x = -1; x <= 2; x++)
            {
                _mapData.AddBuilding(MasterPos +  new Vector2Int(x, y), industriseData.height, industriseData.color);
            }
        }
        materialProductionRatio = Random.Range(0.8f, 3f);
        SetInputeOutpure();
    }
    public void SetInputeOutpure()
    {
        materialsInpute = new Dictionary<MaterialData, int>();
        materialsOutpute = new Dictionary<MaterialData, int>();
        foreach (MaterialData curMaterial in industriseData.materialInpute)
        {
            materialsInpute.Add(curMaterial, 0);
        }
        foreach (MaterialData curMaterial in industriseData.materialOutpute)
        {
            materialsOutpute.Add(curMaterial, 0);
        }
    }

    public void Update(float deltaTime)
    {
        if (productionCoolDown <= 0f)
        {
            ProductMaterial();
            productionCoolDown += 1f / materialProductionRatio;
        }
        productionCoolDown -= deltaTime;
    }

    public void ProductMaterial()
    {
        List<MaterialData> listMaterialsInpute = new List<MaterialData>();
        List<MaterialData> listMaterialsOutpute = new List<MaterialData>();
        foreach(KeyValuePair<MaterialData, int> curMaterial in materialsInpute)
        {
            if (curMaterial.Value != 0)
            {
                listMaterialsInpute.Add(curMaterial.Key);
            }
        }
        foreach (KeyValuePair<MaterialData, int> curMaterial in materialsOutpute)
        {
            if (curMaterial.Value != maxMaterialCanStock)
            {
                listMaterialsOutpute.Add(curMaterial.Key);
            }
        }
        foreach (MaterialData curMaterial in listMaterialsInpute)
        {
            materialsInpute[curMaterial] -= 1;
        }
        foreach (MaterialData curMaterial in listMaterialsOutpute)
        {
            materialsOutpute[curMaterial] += 1;
        }
    }
    
}
