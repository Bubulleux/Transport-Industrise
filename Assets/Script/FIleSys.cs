using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FIleSys
{
    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        return Resources.LoadAll<T>("ScriptableObject");
    }
}
