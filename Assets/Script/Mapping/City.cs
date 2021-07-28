using System;
using System.Collections.Generic;
using Script.Game;
using Script.Mapping.ParcelType;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Mapping
{
    public class City
    {
        public string name;
        public int inhabitantsNumber;
        public Vector2Int masterPos;
        
        private readonly Map mapData;
        private readonly Dictionary<Vector2Int, PlanParcel> cityPlan = new Dictionary<Vector2Int, PlanParcel>();

        private readonly List<Dwelling> dwellings = new List<Dwelling>();
        public City(Vector2Int _pos, Map _mapData)
        {
            masterPos = _pos;
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

        public void Update(float deltaTime)
        {
            foreach (var dwelling in dwellings)
            {
                dwelling.UpdateProduction(deltaTime);
            }
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
                var pos = masterPos + parcel.Key;
                if (mapData.GetparcelType(pos) == typeof(Parcel))
                {
                    switch (parcel.Value)
                    {
                        case PlanParcel.Building:
                            mapData.AddBuilding(pos, 1, Color.gray);
                            dwellings.Add(mapData.GetParcel<Dwelling>(pos));
                            DebugManager.Count("d");
                            break;
                        
                        case PlanParcel.CityCenter:
                            //mapData.AddBuilding(pos, 1, Color.red);
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

                    if (parcel.Value != PlanParcel.None)
                    {
                        mapData.GetParcel(pos).city = this;
                    }
                }
            }
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
