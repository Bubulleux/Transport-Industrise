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
        int i = 0;
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
                        if(!parcelCheck.ContainsKey(curParcel + curParcelAround) && !furureParcelNeedCheck.Contains(curParcel + curParcelAround))
                        {
                            furureParcelNeedCheck.Add(curParcel + curParcelAround);
                        }
                    }
                }
                else
                {
                    if (!parcelCheck.ContainsKey(curParcel))
                    {
                        parcelCheck.Add(curParcel, -1);
                    }
                }
            }
            if (endFind)
            {
                break;
            }
            parcelNeedCheck = furureParcelNeedCheck;
            dist++;
            i++;
            if (i >= 3000)
            {
                Debug.Log("Break 1");
                break;
            }
        }

        if (!endFind)
        {
            return null;
        }

        //List<Vector2Int> returnPath = new List<Vector2Int>();
        //foreach(KeyValuePair<Vector2Int, int> curPointDist in parcelCheck)
        //{
        //    returnPath.Add(curPointDist.Key);
        //}
        //return returnPath;
        List<Vector2Int> path = new List<Vector2Int>() { end };
        i = 0;
        while(path[0] != start)
        {
            foreach(Vector2Int curAroundDir in MapManager.parcelAround)
            {
                Vector2Int curParcelAround = path[0] + curAroundDir;
                if(parcelCheck.ContainsKey(curParcelAround)&& parcelCheck[curParcelAround] != -1 && parcelCheck[curParcelAround] < parcelCheck[path[0]])
                {
                    path.Insert(0, curParcelAround);
                }
            }
            i++;
            if (i >= 1000)
            {
                Debug.Log("Break 2 " + i);
                break;
            }
        }
        return path;

    }
}
