using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractMoney : MonoBehaviour
{
    public delegate bool Condiction();
    public Condiction condiction;
    void Update()
    {
        GetComponent<Button>().interactable = condiction();
    }
}
