using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder 
{
    public static List<Vector2Int> FindPath(Vector2Int start, params Vector2Int[]endPos)
    {
        List<Vector2Int> parcelNeedCheck = new List<Vector2Int>(){ start };
        Dictionary<Vector2Int, int> parcelCheck = new Dictionary<Vector2Int, int>();
        bool endFind = false;
        Vector2Int end = Vector2Int.zero;
        int dist = 0;
        while(parcelNeedCheck.Count != 0)
        {
            List<Vector2Int> furureParcelNeedCheck = new List<Vector2Int>();
            foreach(Vector2Int curParcel in parcelNeedCheck)
            {
                foreach(Vector2Int curEnd in endPos)
                {
                    if (curParcel == curEnd)
                    {
                        endFind = true;
                        end = curParcel;
                        parcelCheck.Add(curParcel, dist);
                        Debug.Log("End: " + curEnd);
                        break;
                    }
                }
                if (endFind)
                {
                    break;
                }
                if (MapManager.map.parcels[curParcel.x, curParcel.y].GetType() == typeof(Road) || MapManager.map.parcels[curParcel.x, curParcel.y].GetType() == typeof(Depot) || MapManager.map.parcels[curParcel.x, curParcel.y].GetType() == typeof(LoadingBay))
                {
                    parcelCheck.Add(curParcel, dist);
                    foreach(Vector2Int curParcelAround in MapManager.parcelAround)
                    {
                        if(!parcelCheck.ContainsKey(curParcel + curParcelAround))
                        {
                            furureParcelNeedCheck.Add(curParcel + curParcelAround);
                        }
                    }
                }
                else
                {
                    parcelCheck.Add(curParcel, -1);
                }
            }
            if (endFind)
            {
                break;
            }
            parcelNeedCheck = furureParcelNeedCheck;
            dist++;
        }

        if (!endFind)
        {
            return null;
        }

        List<Vector2Int> path = new List<Vector2Int>() { end };
        while(path[0] != start)
        {
            foreach(Vector2Int curAroundDir in MapManager.parcelAround)
            {
                Vector2Int curParcelAround = path[0] + curAroundDir;
                if(parcelCheck.ContainsKey(curParcelAround) && parcelCheck[curParcelAround] < parcelCheck[path[0]])
                {
                    path.Insert(0, curParcelAround);
                }
            }
        }
        return path;

    }
}
