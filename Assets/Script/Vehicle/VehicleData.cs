using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vehicle", menuName = "MyGame/Vehicle")]
public class VehicleData : ScriptableObject
{
    public string model;
    public string description;
    public float speed;
    public int price;
    public int maxMaterialTransport;
    public MaterialData[] materialCanTransport;
}
