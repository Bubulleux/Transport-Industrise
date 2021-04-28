using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class Building : Parcel
{
    public float height;
    public Color32 color;
}

