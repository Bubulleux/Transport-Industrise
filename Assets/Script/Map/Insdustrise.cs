using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insdustrise
{
    public TypeIndustrise type;
    public Materials[] materialsInpute;
    public Materials[] materialsOutpute;
    public Vector2Int MasterPos;

    public Insdustrise(Vector2Int _pos, Map _mapData)
    {
        MasterPos = _pos;
        type = (TypeIndustrise)Random.Range(0, 2);
        for (int y = -1; y <= 2; y++)
        {
            for (int x = -1; x <= 2; x++)
            {
                float height = 1f;
                Color color = Color.black;
                switch(type)
                {
                    case TypeIndustrise.mineCoal:
                        color = Color.black;
                        height = 1.5f;
                        break;
                    case TypeIndustrise.stokehole:
                        color = new Color(1f, 0.5f, 0f);
                        height = 3f;
                        break;
                }
                _mapData.AddBuilding(MasterPos +  new Vector2Int(x, y), height, color);
            }
        }
    }
    public void SetInputeOutpure()
    {
        switch(type)
        {
            case TypeIndustrise.mineCoal:
                materialsInpute = new Materials[] { };
                materialsOutpute = new Materials[]{ Materials.coal };
                break;
            case TypeIndustrise.stokehole:
                materialsInpute = new Materials[] { Materials.coal, Materials.wood };
                materialsOutpute = new Materials[] { };
                break;

        }
    }

    public enum TypeIndustrise
    {
        mineCoal,
        stokehole
    }
    
}
public enum Materials
{
    coal,
    wood,
    fer
}