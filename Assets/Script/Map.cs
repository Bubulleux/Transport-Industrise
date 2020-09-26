using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instence;
    public GameObject roadPrefab;
    public Parcel[,] parcels;

    private void Awake()
    {
        instence = this;
    }

    void Start()
    {
        parcels = new Parcel[100, 100];
    }
    
    void Update()
    {
        
    }
    public bool AddRoad(Vector2Int pos)
    {
        if (parcels[pos.x, pos.y] == null)
        {
            Instantiate(roadPrefab, new Vector3(pos.x, 0f, pos.y), Quaternion.identity, transform);
            parcels[pos.x, pos.y] = new Road();
            Debug.LogFormat("Road Make in {0}", pos);
            return true;
        }
        return false;
    }
}

public class Parcel
{
    Vector2Int pos;
}