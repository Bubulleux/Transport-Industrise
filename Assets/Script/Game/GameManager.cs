using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instence;

    [SerializeField]
    private long money = 0;
    public static long Money 
    {
        get { return instence.money; } 
        set 
        {
            instence.money = value;
        } 
    }

    void Awake()
    {
        instence = this;
        money = 100000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
