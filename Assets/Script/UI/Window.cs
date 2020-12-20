using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool pointerOverMe = false;
    public bool fixWindow;
    public Transform Contente { get => transform.Find("WindowContent"); }

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
