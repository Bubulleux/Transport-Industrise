using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    static DontDestroyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
