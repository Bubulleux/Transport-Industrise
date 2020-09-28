using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.UI;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instence;
    public GameObject roadPrefab;
    public Parcel[,] parcels;
    public Transform gfx;

    private void Awake()
    {
        instence = this;
    }

    void Start()
    {
        parcels = new Parcel[201, 201];
        GenerateMap();
    }
    
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        Debug.ClearDeveloperConsole();
        float[,] noise = NoiseGenerator.GenerNoise(201 , 201, 10, 3, 6, 0.1f, 10, AnimationCurve.Linear(0, 0, 20, 20));
        Mesh mesh = MeshGenerator.MeshGenerat(noise).GetMesh();
        gfx.GetComponent<MeshFilter>().sharedMesh = mesh;
        gfx.GetComponent<MeshCollider>().sharedMesh = mesh;
        //Texture2D texture = TerxtureGennerator.GenerTexture(noise, TerxtureGennerator.TexturRenderType.blackAndWhite, new TerxtureGennerator.region[0]);
        //plane.sharedMaterial.mainTexture = texture;
        //plane.transform.localScale = new Vector3(mapScale.x , 1f, mapScale.y);
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