using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[JsonObject(MemberSerialization.OptOut)]
public class Parcel
{
    public Vector2Int pos;
    public int[] corner = new int[4];
    //public object construction = null;
    public bool seeTerrain = true;

    public Parcel(Vector2Int _pos)
    {
        pos = _pos;
    }
    public Parcel() { }

    public virtual void Initialaze()
    {

    }
    
    public virtual void Interact()
    {

    }

    public virtual void DebugParcel()
    {
        UnityEngine.Debug.Log(pos);
        string parcelJson = Save.GetJson(MapManager.map.GetParcel(pos));
        Debug.Log(parcelJson);
    }

    public static Parcel CopyClass(Parcel copyClass,Parcel pastClass)
    {
        pastClass.pos = copyClass.pos;
        pastClass.corner = copyClass.corner;
        pastClass.seeTerrain = copyClass.seeTerrain;
        pastClass.Initialaze();
        return pastClass;
    }

}

[JsonObject(MemberSerialization.OptOut)]
public class Road : Parcel
{
    public bool[] direction = new bool[4];
}

[JsonObject(MemberSerialization.OptOut)]
public class Building : Parcel
{
    public float height;
    public Color32 color;
}


[JsonObject(MemberSerialization.OptOut)]
public class Depot : Parcel
{

    public VehicleContoler BuyVehicle(VehicleData vehicle)
    {
        GameObject _go = Object.Instantiate(Resources.Load("Vehicle") as GameObject);
        _go.transform.position = new Vector3(pos.x , 0f, pos.y);
        _go.GetComponent<VehicleContoler>().vehicleData = vehicle;
        return _go.GetComponent<VehicleContoler>();
    }
    public override void Interact()
    {
        base.Interact();
        WindowsOpener.OpenDepotWindow(this);
    }
}


[JsonObject(MemberSerialization.OptOut)]
public class LoadingBay : Parcel
{
    public List<Industrise> industriseLink = new List<Industrise>();
    public override void Initialaze()
    {
        foreach(Industrise curIndustrise in MapManager.map.industrises)
        {
            if (Vector2Int.Distance(pos, curIndustrise.MasterPos) <= 20)
            {
                industriseLink.Add(curIndustrise);
            }
        }
        Debug.Log($"Industrise link: {industriseLink.Count}");
    }
    public override void DebugParcel()
    {
        base.DebugParcel();
        string result = "Inpute:";
        foreach (KeyValuePair<MaterialData, int> curMaterial in GetMaterialInput())
        {
            result += $"\n{curMaterial.Key}: {curMaterial.Value}";
        }
        result += "\nOutpute: ";
        foreach (KeyValuePair<MaterialData, int> curMaterial in GetMaterialOutpute())
        {
            result += $"\n{curMaterial.Key}: {curMaterial.Value}";
        }
        Debug.Log(result);
    
    }
    public override void Interact()
    {
        base.Interact();
        WindowsOpener.OpenLoadingBay(this);
    }

    public int GetMaterial(MaterialData material)
    {
        int resulte = 0;
        foreach(Industrise curIndustrise in industriseLink)
        {
            if (curIndustrise.materialsOutpute.ContainsKey(material))
            {
                resulte += curIndustrise.materialsOutpute[material];
            }
        }
        return resulte;
    }

    public Dictionary<MaterialData, int> GetMaterialInput()
    {
        Dictionary<MaterialData, int> resulte = new Dictionary<MaterialData, int>();
        foreach (Industrise curIndustrise in industriseLink)
        {
            foreach(KeyValuePair<MaterialData, int> curMaterial in curIndustrise.materialsInpute)
            {
                if (resulte.ContainsKey(curMaterial.Key))
                {
                    resulte[curMaterial.Key] += curMaterial.Value;
                }
                else
                {
                    resulte.Add(curMaterial.Key, curMaterial.Value);
                }
            }
        }
        return resulte;
    }

    public Dictionary<MaterialData, int> GetMaterialOutpute()
    {
        Dictionary<MaterialData, int> resulte = new Dictionary<MaterialData, int>();
        foreach (Industrise curIndustrise in industriseLink)
        {
            foreach (KeyValuePair<MaterialData, int> curMaterial in curIndustrise.materialsOutpute)
            {
                if (resulte.ContainsKey(curMaterial.Key))
                {
                    resulte[curMaterial.Key] += curMaterial.Value;
                }
                else
                {
                    resulte.Add(curMaterial.Key, curMaterial.Value);
                }
            }
        }
        return resulte;
    }

    public Dictionary<MaterialData, float> GetMaterialRatio(bool getInput)
    {
        Dictionary<MaterialData, int> materialQuantity = new Dictionary<MaterialData, int>();
        Dictionary<MaterialData, int> materialCount = new Dictionary<MaterialData, int>();
        foreach (Industrise curIndustrise in industriseLink)
        {
            foreach (KeyValuePair<MaterialData, int> curMaterial in getInput ? curIndustrise.materialsInpute : curIndustrise.materialsOutpute)
            {
                if (materialQuantity.ContainsKey(curMaterial.Key))
                {
                    materialQuantity[curMaterial.Key] += curMaterial.Value;
                    materialCount[curMaterial.Key] += Industrise.maxMaterialCanStock;
                }
                else
                {
                    materialQuantity.Add(curMaterial.Key, curMaterial.Value);
                    materialCount.Add(curMaterial.Key, Industrise.maxMaterialCanStock);
                }
            }
        }
        Dictionary<MaterialData, float> result = new Dictionary<MaterialData, float>();
        foreach(KeyValuePair<MaterialData, int> curMaterial in materialQuantity)
        {
            result.Add(curMaterial.Key, curMaterial.Value / (float)materialCount[curMaterial.Key]);
        }
        return result;
    }

    public int GiveOrTakeMaterial(MaterialData material, int quantity)
    {
        int _quantity = quantity;
        foreach(Industrise curIndustrise in industriseLink)
        {
            if(curIndustrise.materialsInpute.ContainsKey(material))
            {
                curIndustrise.materialsInpute[material] += _quantity;
                _quantity = 0;
                if (curIndustrise.materialsInpute[material]-Industrise.maxMaterialCanStock > 0)
                {
                    _quantity = curIndustrise.materialsInpute[material] - Industrise.maxMaterialCanStock;
                    curIndustrise.materialsInpute[material] = Industrise.maxMaterialCanStock;
                }

                if (curIndustrise.materialsInpute[material] < 0)
                {
                    quantity = curIndustrise.materialsInpute[material];
                    curIndustrise.materialsInpute[material] = 0;
                }
            }
        }
        return _quantity;
    }

    public bool CanUnload(MaterialData material)
    {
        foreach(Industrise curIndustrise in industriseLink)
        {
            if(curIndustrise.materialsInpute.ContainsKey(material) && curIndustrise.materialsInpute[material] != Industrise.maxMaterialCanStock)
            {
                return true;
            }
        }
        return false;
    }

    public int TryToInteract(MaterialData material, int materialQuantityGive)
    {
        int materialHasNotGiven = materialQuantityGive;
        foreach(Industrise curIndustrise in industriseLink)
        {
            if (materialHasNotGiven == 0)
            {
                break;
            }
            if (materialHasNotGiven > 0 && curIndustrise.materialsInpute.ContainsKey(material) && curIndustrise.materialsInpute[material] != Industrise.maxMaterialCanStock)
            {
                curIndustrise.materialsInpute[material] += materialHasNotGiven;
                if (curIndustrise.materialsInpute[material] > Industrise.maxMaterialCanStock)
                {
                    materialHasNotGiven = curIndustrise.materialsInpute[material] - Industrise.maxMaterialCanStock;
                    curIndustrise.materialsInpute[material] = Industrise.maxMaterialCanStock;
                }
                else
                {
                    materialHasNotGiven = 0;
                }
            }
            Debug.Log(curIndustrise.materialsOutpute.ContainsKey(material));
            Debug.Log(curIndustrise.materialsOutpute[material] != 0);
            if (materialHasNotGiven < 0 && curIndustrise.materialsOutpute.ContainsKey(material) && curIndustrise.materialsOutpute[material] != 0)
            {
                curIndustrise.materialsOutpute[material] += materialHasNotGiven;
                if (curIndustrise.materialsOutpute[material] < 0)
                {
                    materialHasNotGiven = curIndustrise.materialsOutpute[material];
                    curIndustrise.materialsOutpute[material] = 0;
                }
                else
                {
                    materialHasNotGiven = 0;
                }
            }

        }
        return materialQuantityGive - materialHasNotGiven;
    }
}