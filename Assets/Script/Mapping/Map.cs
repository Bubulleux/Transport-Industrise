using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Script.MapGeneration;
using Script.Mapping.ParcelType;
using Script.Vehicle;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Mapping
{
	[JsonObject(MemberSerialization.OptIn)]
	public  class Map
	{
		[JsonProperty]
		public Parcel[,] parcels;
		public Vector2Int Size { get => new Vector2Int(parcels.GetLength(0), parcels.GetLength(1)); }
		[JsonProperty]
		public List<City> citys = new List<City>();
		[JsonProperty]
		public List<Industrise> industrises =  new List<Industrise>();

		public  const int ChuckSize = 50;


	
		public List<Parcel> importParcels = new List<Parcel>();
		public List<Parcel> updatedParcel = new List<Parcel>();

		public MapManager Manager;

		public Map(Vector2Int chunkSize)
		{
			parcels = new Parcel[chunkSize.x * 50, chunkSize.y * 50];
		}

	
		public async Task GenerateMap(MapSettingData mapSetting)
		{
			parcels = new Parcel[1000, 1000];
			await Task.Delay(10);
			citys = new List<City>();
			industrises = new List<Industrise>();
			var mapNoise = NoiseGenerator.GenerNoise(1001, 1001, 60, 3, 6, 0.1f, Random.Range(1, 10000), mapSetting.heightCurv, mapSetting.limitWaterCurv);

			var biomesNoise = new Dictionary<BiomeData, float[,]>();
			foreach (var biome in FIleSys.GetAllInstances<BiomeData>())
			{
				var noiseBiome = NoiseGenerator.GenerNoise(1000, 1000, 100, 2, 6, 0.1f, Random.Range(1, 99999));
				biomesNoise.Add(biome, noiseBiome);
			}
		
			for (var y = 0; y < parcels.GetLength(1); y++)
			{
				for (var x = 0; x < parcels.GetLength(0); x++)
				{
					BiomeData parcelBiome = null;
					foreach (var value in biomesNoise)
					{
						if (parcelBiome == null)
						{
							parcelBiome = value.Key;
							continue;
						}
						if (biomesNoise[parcelBiome][x, y] * parcelBiome.coef < value.Value[x, y] * value.Key.coef)
						{
							parcelBiome = value.Key;
						}
					}
				
					parcels[x, y] = new Parcel(new Vector2Int(x, y), this)
					{
						corner = new []
						{
							Mathf.FloorToInt(mapNoise[x + 1, y + 1]),
							Mathf.FloorToInt(mapNoise[x + 1, y + 0]),
							Mathf.FloorToInt(mapNoise[x + 0, y + 0]),
							Mathf.FloorToInt(mapNoise[x + 0, y + 1]),
						},
						biome = parcelBiome,
					};
					parcels[x, y].Initialaze();
				}
			}
			await Task.Delay(10);
			while (citys.Count < 20)
			{
				await Task.Delay(10);;
				CreatCity(Random.Range(100, 900), Random.Range(100, 900));
			}
			for (var i = 0; i < 50; i++)
			{
				CreatIndustrise(new Vector2Int(Random.Range(100, 900), Random.Range(100, 900)));
				await Task.Delay(10);
			}
		}

		public T GetParcel<T>(Vector2Int pos) where T : Parcel
		{
			Parcel parcel = GetParcel(pos);
			if ( parcel is T)
				return parcel as T;
			return null;
		}
		public Parcel GetParcel(Vector2Int pos)
		{
			if (pos.x < 0 || pos.x >= parcels.GetLength(0) ||
			    pos.y < 0 || pos.y >= parcels.GetLength(1))
			{
				return new Parcel(pos, this);
			}
			return parcels[pos.x, pos.y];
		}

		public System.Type GetparcelType(Vector2Int pos)
		{
			return parcels[pos.x, pos.y].GetType();
		}

		public bool ParcelIs<T>(Vector2Int pos)
		{
			return GetParcel(pos) is T;
		}

		public bool CreatCity(int x, int y)
		{
			foreach (var _city in citys)
			{
				if (Vector2Int.Distance(new Vector2Int(x, y), _city.MasterPos) < 100f)
				{
					return false;
				}
			}
			//Transform _go = new GameObject().transform;
			//_go.position = new Vector3(x, 0f, y);
			//_go.parent = GameObject.Find("Citys").transform;
			var city = new City(new Vector2Int(x, y), this)
			{
				name = "City " + citys.Count,
				inhabitantsNumber = Random.Range(100, 1999)
			};
			citys.Add(city);
			return true;
		}

		public Industrise CreatIndustrise(Vector2Int pos)
		{
			foreach (var _city in citys)
			{
				if (Vector2Int.Distance(pos, _city.MasterPos) < 50f)
				{
					return null;
				}
			}
			foreach (var _industrise in industrises)
			{
				if (Vector2Int.Distance(pos, _industrise.MasterPos) < 10f)
				{
					return null;
				}
			}
			//Transform _go = new GameObject().transform;
			//_go.position = new Vector3(x, 0f, y);
			//_go.parent = GameObject.Find("Instrises").transform;
			//_go.name = "Industrise " + industrises.Count;
			var Industrise = new Industrise(pos, this);
			industrises.Add(Industrise);
			return Industrise;
		}

		public bool AddRoad(Vector2Int pos)
		{
			if (pos.x < 50 || pos.x >= 950 || pos.y < 50 || pos.y >= 950)
			{
				Debug.Log("Return");
				return false;
			}
			if (parcels[pos.x, pos.y].GetType() == typeof(Parcel))
			{
				for (var j = 0; j < MapManager.parcelAroundCorner.Length; j++)
				{
					if (MapManager.parcelAroundCorner[j].x == 0 || MapManager.parcelAroundCorner[j].y == 0)
					{
						continue;
					}
					var _countRoad = 0;
					for (var h = -1; h <= 1; h++)
					{
						var _j = (j + h) % MapManager.parcelAroundCorner.Length;
						if (_j == -1)
						{
							_j = MapManager.parcelAroundCorner.Length - 1;
						}
						if (parcels[MapManager.parcelAroundCorner[_j].x + pos.x, MapManager.parcelAroundCorner[_j].y + pos.y].GetType() != typeof(Parcel) && parcels[MapManager.parcelAroundCorner[_j].x + pos.x, MapManager.parcelAroundCorner[_j].y + pos.y].GetType() == typeof(Road))
						{
							_countRoad++;
						}
						if (_countRoad == 3)
						{
							return false;
						}
					}
				}
				//Instantiate(roadPrefab, new Vector3(pos.x, parcels[pos.x, pos.y].corner[0], pos.y), Quaternion.identity, transform);
				parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], new Road());
				foreach (var neightbourPos in MapManager.parcelAround)
				{
					if (GetparcelType(pos + neightbourPos) == typeof(Road))
					{
						GetParcel<Road>(pos + neightbourPos).UpdateRoadObject();
					}
				}
				//parcels[pos.x, pos.y].seeTerrain = false;
				UpdateChunkTexture(pos);
				UpdateChunkObject(pos);
				//if (mapHeight[pos.x, pos.y] != mapHeight[pos.x, pos.y + 1] || mapHeight[pos.x + 1 , pos.y] != mapHeight[pos.x + 1 , pos.y + 1] || mapHeight[pos.x, pos.y] != mapHeight[pos.x + 1, pos.y])
				//{
				//    mapHeight[pos.x, pos.y + 1] = mapHeight[pos.x, pos.y];
				//    mapHeight[pos.x + 1, pos.y + 1] = mapHeight[pos.x, pos.y];
				//    mapHeight[pos.x + 1, pos.y] = mapHeight[pos.x, pos.y];
				//}
				return true;

			}
			return false;
		}

		public bool AddBuilding(Vector2Int pos, float height, Color color)
		{ 
			if (parcels[pos.x, pos.y].GetType() == typeof(Parcel))
			{
				parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], new Dwelling());
				GetParcel(pos).color = color;
				importParcels.Add(GetParcel(pos));
				//parcels[pos.x, pos.y].seeTerrain = false;
				UpdateChunkMesh(pos);
				return true;

			}
			return false;
		}

		public bool AddConstruction(Vector2Int pos, Parcel construction)
		{
			parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], construction);
			UpdateChunkTexture(pos);
			importParcels.Add(construction);
			return true;
		}

		public bool Destroy(Vector2Int pos)
		{
			if (GetparcelType(pos) == typeof(Parcel) || (GetparcelType(pos) == typeof(Road) && VehicleManager.GetVehicleByPos(GetParcel<Parcel>(pos)).Count != 0))
			{
				return false;
			}
			parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], new Parcel());
			UpdateChunkTexture(pos);
			UpdateChunkObject(pos);
			importParcels.Remove(parcels[pos.x, pos.y]);
			return true;
		}

		public void Color(Vector2Int pos, Color color)
		{
			var building = new Building
			{
				color = color,
				height = 0
			};
			parcels[pos.x, pos.y] = Parcel.CopyClass(parcels[pos.x, pos.y], building);
			//parcels[pos.x, pos.y].seeTerrain = false;
			UpdateChunkTexture(pos);
		}

		
		public void UpdateChunkTexture(Vector2Int pos)
		{
			if (Manager)
				Manager.ChuckObjects[Mathf.FloorToInt(pos.x / 50f), Mathf.FloorToInt(pos.y / 50f)].NeedTextureUpdate = true;
		}

		public void UpdateChunkMesh(Vector2Int pos)
		{
			if (Manager)
				Manager.ChuckObjects[Mathf.FloorToInt(pos.x / 50f), Mathf.FloorToInt(pos.y / 50f)].NeedMeshUpdate = true;
		}
		public void UpdateChunkObject(Vector2Int pos)
		{
			if (Manager)
				Manager.ChuckObjects[Mathf.FloorToInt(pos.x / 50f), Mathf.FloorToInt(pos.y / 50f)].NeedObjectUpdate = true;
		}

		
		public T[] GetImpotantParcel<T>() where T : Parcel
		{
			var parcelsList = new List<T>();
			foreach(var curParcel in importParcels)
			{
				if (curParcel.GetType() == typeof(T))
				{
					parcelsList.Add(curParcel as T);
				}
			}
			var parcelArray = new T[parcelsList.Count];

			for (var i = 0; i < parcelsList.Count; i++)
				parcelArray[i] = parcelsList[i];
		
			return parcelArray;

		}

		public void SetParcelCorner(Vector2Int pos, int[] corners, int recursion = 0)
		{
			for (int i = 0; i < 4; i++)
			{
				SetHeight(pos + MapManager.cornerAround[i], corners[i]);
			}
		}

		public void SetHeight(Vector2Int pos, int height)
		{
			for (int i = 0; i < 4; i++)
			{
				GetParcel(pos - MapManager.cornerAround[i]).corner[i] = height;
				UpdateChunkMesh(pos - MapManager.cornerAround[i]);
			}
			
		}
	}
}
