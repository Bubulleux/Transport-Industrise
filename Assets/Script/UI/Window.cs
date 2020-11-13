using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool pointerOverMe = false;
    public bool fixWindow;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        Destroy(gameObject);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (! fixWindow)
        {
            transform.SetAsLastSibling();
            GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOverMe = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOverMe = false;
    }
}
