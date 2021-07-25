using System;
using System.Collections.Generic;
using Script.Mapping.ParcelType;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Mapping
{
    public class City
    {
        public string name;
        public int inhabitantsNumber;
        public Vector2Int MasterPos;
        private Dictionary<Vector2Int, parcelStatus> parcelsCityStatus = new Dictionary<Vector2Int, parcelStatus>();
        public Map mapData;
        private Dictionary<Vector2Int, PlanParcel> cityPlan = new Dictionary<Vector2Int, PlanParcel>();
        public City(Vector2Int _pos, Map _mapData)
        {
            MasterPos = _pos;
            mapData = _mapData;
            for (int y = -2; y < 3; y++)
            {
                for (int x = -2; x < 3; x++)
                {
                    if (y == 2 || y == -2 || x == -2 || x == 2)
                    {
                        cityPlan[new Vector2Int(x, y)] = PlanParcel.CityCenterRoad;
                    }
                    else 
                    {
                        cityPlan[new Vector2Int(x, y)] = PlanParcel.CityCenter;
                    }
                }
            }
            
            int size = Random.Range(10, 50);
            if (size % 2 == 0)
                size += 1;
            
            GenerateMainStreets(size);
            GenerateAllStreets(size);
            
            BuildCity();

        }

        private void GenerateMainStreets(int size)
        {
            for (int sense = 0; sense < 2; sense++)
            {
                for (int i = -size / 2; i <=  size / 2; i++)
                {
                    var pos = new Vector2Int(0, i);
                    if (sense == 1)
                        pos = new Vector2Int(pos.y, pos.x);
                    MakeRoad(pos, sense == 1);
                }
            }
        }

        private void GenerateAllStreets(int size)
        {
            for (int sense = 0; sense < 2; sense++)
            {
                var roadArray = GetStreetArray(size, 0.3f);
                for (int street = 0; street < size; street++)
                {
                    if (!roadArray[street])
                        continue;
                    
                    var sizeA = Mathf.FloorToInt(Random.Range(0.1f, 0.6f) * -size * Mathf.Cos(street * Mathf.PI * 0.8f / size - Mathf.PI / 2));
                    var sizeB = Random.Range(0.1f, 0.6f) * size * Mathf.Cos(street * Mathf.PI * 0.8f / size - Mathf.PI / 2);
                    
                    for (int i = sizeA; i < sizeB; i++)
                    {
                        var pos = new Vector2Int(street - size / 2, i);
                        if (sense == 1)
                            pos = new Vector2Int(pos.y, pos.x); 
                        MakeRoad(pos, sense == 1);
                    }
                }
            }
        }

        private bool[] GetStreetArray(int size, float roadChance)
        {
            var roadArray = new bool[size];
            var canRoad = 0;
            for (int i = 0; i < size; i++)
            {
                if (canRoad > 0 || (i - size / 2 >= -1 && i - size / 2 <= 1))
                {
                    //Debug.Log($"{i}, {size}, {canRoad}");
                    canRoad -= 1;
                    continue;
                }

                if (Random.value < roadChance)
                {
                    roadArray[i] = true;
                    canRoad = Random.value > 0.3 ? 1 : 2;
                }
            }
            
            return roadArray;
        }

        private void MakeRoad(Vector2Int pos, bool xAxe)
        {
            if (!cityPlan.ContainsKey(pos) || cityPlan[pos] == PlanParcel.Building)
            {
                cityPlan[pos] = PlanParcel.Road;
                for (int i = 1; i <= 3; i++)
                {
                    if (Random.value > 0.3f)
                    {
                        var b1 = MakeBuilding(pos + (xAxe ? new Vector2Int(0, i) : new Vector2Int(i, 0)));
                        var b2 = MakeBuilding(pos + (xAxe ? new Vector2Int(0, -i) : new Vector2Int(-i, 0)));
                        
                        if (!b1 || !b2)
                            break;
                    }
                    else
                        break;
                }
            }
        }

        private bool MakeBuilding(Vector2Int pos)
        {
            if (!cityPlan.ContainsKey(pos))
            {
                cityPlan[pos] = PlanParcel.Building;
                return true;
            }

            return false;
        }
        
        private void BuildCity()
        {
            foreach (var parcel in cityPlan)
            {
                var pos = MasterPos + parcel.Key;
                if (mapData.GetparcelType(pos) == typeof(Parcel))
                {
                    switch (parcel.Value)
                    {
                        case PlanParcel.Building:
                            mapData.AddBuilding(pos, 1, Color.gray);
                            break;
                        case PlanParcel.CityCenter:
                            mapData.AddBuilding(pos, 1, Color.red);
                            break;
                        case PlanParcel.Road:
                        case PlanParcel.CityCenterRoad:
                            mapData.AddRoad(pos);
                            break;
                        case PlanParcel.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        
        
        // public void GenerateStreet()
        // {
        //     List<Vector2Int> parcelCanRoad = GetParcels(parcelStatus.canRoad);
        //     Vector2Int mainRoad = parcelCanRoad[Random.Range(0, parcelCanRoad.Count)];
        //     //for (int i = 0; i < 1; i++)
        //     //{
        //     //    Vector2Int v = parcelCanRoad[Random.Range(0, parcelCanRoad.Count)];
        //     //    if (Vector2Int.Distance(mainRoad, Vector2Int.zero) < Vector2Int.Distance(v, Vector2Int.zero))
        //     //    {
        //     //        mainRoad = v;
        //     //    }
        //     //}
        //
        //     Vector2Int dir = Vector2Int.zero;
        //     foreach (Vector2Int _dir in MapManager.parcelAround)
        //     {
        //         if (parcelsCityStatus.ContainsKey(mainRoad + _dir) && parcelsCityStatus[mainRoad + _dir] == parcelStatus.road)
        //         {
        //             dir = Vector2Int.zero - _dir;
        //         }
        //     }
        //     //dir = MapManager.parcelAround[Random.Range(0, 4)];
        //     for (int i = 0; i < Random.Range(2, 6); i++)
        //     {
        //         Vector2Int _road = mainRoad + (dir * i);
        //         //Debug.LogFormat("road : {0}, dir: {1}, i: {2}, main : {3}", _road, dir, i, mainRoad);
        //         parcelsCityStatus[_road] = parcelStatus.road;
        //         if (!mapData.AddRoad(MasterPos + _road))
        //         {
        //             break;
        //         }
        //         if (i != 0)
        //         {
        //             parcelsCityStatus[_road + new Vector2Int(dir.y, dir.x)] = parcelStatus.canRoad;
        //             parcelsCityStatus[_road + new Vector2Int(dir.y, dir.x) * -1] = parcelStatus.canRoad;
        //         }
        //     }
        // }

        public void GenerateBuilding()
        {
            List<Vector2Int> parcelCanBuild = GetParcels(parcelStatus.canRoad);
            Vector2Int pos = parcelCanBuild[Random.Range(0, parcelCanBuild.Count)];
            for (int i = 0; i < 5; i++)
            {
                Vector2Int v = parcelCanBuild[Random.Range(0, parcelCanBuild.Count)];
                if (Vector2Int.Distance(pos, Vector2Int.zero) > Vector2Int.Distance(v, Vector2Int.zero))
                {
                    pos = v;
                }
            }
            Vector2Int minVec = new Vector2Int(-2, -2);
            Vector2Int maxVec = new Vector2Int(2, 2);
            for (int y = -3; y <= 3; y++)
            {
                for (int x = -3; x <= 3; x++)
                {
                    if (parcelsCityStatus.ContainsKey(pos + new Vector2Int(x, y)) && parcelsCityStatus[pos + new Vector2Int(x, y)] != parcelStatus.canRoad)
                    {
                        if (y < 0 && minVec.y <= y)
                        {
                            minVec = new Vector2Int(minVec.x, y + 1);
                        }
                        else if(y > 0 && maxVec.y >= y)
                        {
                            maxVec = new Vector2Int(maxVec.x, y - 1);
                        }
                        else if (y == 0 && minVec.x <= x && x < 0)
                        {
                            minVec = new Vector2Int(x + 1, minVec.y);
                        }
                        else if (y == 0 && maxVec.x >= x && x > 0)
                        {
                            maxVec = new Vector2Int(x - 1, maxVec.y);
                        }
                    }
                }
            }
            float height = Random.Range(0.7f, 5f);
            for (int y = minVec.y; y <= maxVec.y; y++)
            {
                for (int x = minVec.x; x <= maxVec.x; x++)
                {
                    if (!mapData.AddBuilding(pos+ MasterPos + new Vector2Int(x, y), height, Color.grey))
                    {
                        //Debug.LogFormat("Pos {0}, min {1}, max {2}, try {3}", pos + pos, minVec, maxVec, pos + pos + new Vector2Int(x, y));
                    }
                    parcelsCityStatus[pos + new Vector2Int(x, y)] = parcelStatus.construction;
                }
            }

        }
    
        public void ReCalcCanRoad()
        {
            foreach(Vector2Int road in GetParcels(parcelStatus.road))
            {
                foreach(Vector2Int dir in MapManager.parcelAround)
                {
                    if(!parcelsCityStatus.ContainsKey(dir + road))
                    {
                        parcelsCityStatus.Add(dir + road, parcelStatus.canRoad);
                        continue;
                    }
                    if(mapData.parcels[MasterPos.x + road.x + dir.x, MasterPos.y + road.y + dir.y].GetType() == typeof(Parcel))
                    {
                        parcelsCityStatus[dir + road]= parcelStatus.canRoad;
                    }
                }
            }
        }

        public List<Vector2Int> GetParcels(parcelStatus status)
        {
            List<Vector2Int> returnValue = new List<Vector2Int>();
            foreach (KeyValuePair<Vector2Int, parcelStatus> parcel in parcelsCityStatus)
            {
                if (parcel.Value == status)
                {
                    returnValue.Add(parcel.Key);
                }
            }
            return returnValue;
        }
        public enum parcelStatus
        {
            canRoad,
            construction,
            road,
        }

        private enum PlanParcel
        {
            None,
            Road,
            Building,
            CityCenter,
            CityCenterRoad,
            
        }
    }
}
