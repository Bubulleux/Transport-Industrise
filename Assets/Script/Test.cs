using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        List<int> list = new List<int>() { 1, 2, 3, 4 };
        list.Insert(0, 0);
        string pr = "";
        foreach(int i in list)
        {
            pr = pr + "  " + i;
        }
        Debug.Log(pr);
    }
}


