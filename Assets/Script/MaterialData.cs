using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "MyGame/Material")]
public class MaterialData : ScriptableObject
{
    public string name;
    public int buyPrice;
    public int sellPrice;
}
