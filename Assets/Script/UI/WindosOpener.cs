using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindosOpener 
{
    public static GameObject openDepotWindow(Vector2Int pos)
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/DepotWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        _go.GetComponent<DepotWindow>().Init(pos);
        return _go;
    }

    public static GameObject openMarketWindow(Vector2Int depotPos)
    {
        GameObject _go = Object.Instantiate(Resources.Load("UI/MarketWindow", typeof(GameObject)) as GameObject);
        _go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _go.transform.localPosition = Vector3.zero;
        return _go;
    }
}
