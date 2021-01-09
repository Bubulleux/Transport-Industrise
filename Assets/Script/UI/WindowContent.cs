using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowContent : MonoBehaviour
{
    public Window WindowParente
    {
        get
        {
            return transform.parent.GetComponent<Window>();
        }
    }

    public void Close()
    {
        WindowParente.Close();
    }
}
