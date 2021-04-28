using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class Road : Parcel
{
    public bool[] direction = new bool[4];
}
