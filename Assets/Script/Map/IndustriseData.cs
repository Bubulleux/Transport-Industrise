using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Industrise", menuName = "MyGame/Industrise")]
public class IndustriseData : ScriptableObject
{
    public string name;
    public Materials[] materialInpute;
    public Materials[] materialOutpute;
    public Color color;
    public float height;
}
