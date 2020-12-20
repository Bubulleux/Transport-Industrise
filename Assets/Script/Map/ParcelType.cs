using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Parcel
{
    public Vector2Int pos;
    public int[] corner = new int[4];
    //public object construction = null;
    public bool seeTerrain = true;

    public virtual void Initialaze()
    {

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

public class Road : Parcel
{
    public bool[] direction = new bool[4];
}

public class Building : Parcel
{
    public float height;
    public Color color;
}



public class Depot : Parcel
{

    public VehicleContoler BuyVehicle(VehicleData vehicle)
    {
        GameObject _go = Object.Instantiate(Resources.Load("Vehicle") as GameObject);
        _go.transform.position = new Vector3(pos.x , 0f, pos.y);
        _go.GetComponent<VehicleContoler>().vehicleData = vehicle;
        return _go.GetComponent<VehicleContoler>();
    }
}

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

    public int GetMaterial(Materials material)
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

    public Dictionary<Materials, int> GetMaterialInput()
    {
        Dictionary<Materials, int> resulte = new Dictionary<Materials, int>();
        foreach (Industrise curIndustrise in industriseLink)
        {
            foreach(KeyValuePair<Materials, int> curMaterial in curIndustrise.materialsInpute)
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

    public Dictionary<Materials, int> GetMaterialOutpute()
    {
        Dictionary<Materials, int> resulte = new Dictionary<Materials, int>();
        foreach (Industrise curIndustrise in industriseLink)
        {
            foreach (KeyValuePair<Materials, int> curMaterial in curIndustrise.materialsOutpute)
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

    public int GiveOrTakeMaterial(Materials material, int quantity)
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

    public bool CanUnload(Materials material)
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

    public int TryToInteract(Materials material, int materialQuantityGive)
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