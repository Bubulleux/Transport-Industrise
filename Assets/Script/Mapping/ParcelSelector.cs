using System;
using System.Collections.Generic;
using Script.MapGeneration;
using Script.Mapping.ParcelType;
using UnityEngine;

namespace Script.Mapping
{
	public class ParcelSelector : MonoBehaviour
	{
		private readonly Dictionary<Vector2Int, Color> _parcelSeleced = new Dictionary<Vector2Int, Color>();
		private readonly Dictionary<Vector2Int, Renderer> _gameObjectList = new Dictionary<Vector2Int, Renderer>();
		private List<Renderer> _boxUpdated = new List<Renderer>();

		private bool _updateSelection = false;

		public GameObject SelectBox;
		public Transform SelectBoxParente;

		public int BoxSelectioCount;

		public bool SelectParcel(Vector2Int pos, Color color, bool force = false)
		{
			if (_parcelSeleced.Count >= 2000)
				return false;
			
			if (_parcelSeleced.ContainsKey(pos) && force)
				_parcelSeleced[pos] = color;
			else if (!_parcelSeleced.ContainsKey(pos))
				_parcelSeleced.Add(pos, color);
			
			_updateSelection = true;
			
			return true;
		}

		public void SelectArea(Vector2Int posA, Vector2Int posB, Color color, bool aroundOnly = true)
		{
			foreach (var pos in Helper.GetArea(posA, posB, aroundOnly))
			{
				SelectParcel(pos, color);
			}
		}

		public void UnSelectParcel(Vector2Int pos)
		{
			if (_parcelSeleced.ContainsKey(pos))
			{
				_parcelSeleced.Remove(pos);
				_updateSelection = true;
			}
		}

		public void UnSelectColor(Color color)
		{
			var needRemove = new List<Vector2Int>();
			foreach (var parcel in _parcelSeleced)
			{
				if (parcel.Value == color)
					needRemove.Add(parcel.Key);
			}

			foreach (var parcel in needRemove)
			{
				_parcelSeleced.Remove(parcel);
			}

			_updateSelection = true;
		}

		public void ClearSelection()
		{
			_parcelSeleced.Clear();
			_updateSelection = true;
		}

		public void OnRenderObject()
		{
			if (_updateSelection)
			{
				_boxUpdated.Clear();
				_updateSelection = false;

				List<Vector2Int> parcelNeedObject = new List<Vector2Int>();
				
				foreach (var parcel in _parcelSeleced)
				{
					if (_gameObjectList.ContainsKey(parcel.Key))
					{
						UpdateBox(parcel.Key, parcel.Value, _gameObjectList[parcel.Key]);
					}
					else
					{
						parcelNeedObject.Add(parcel.Key);
					}
				}

				var cachObject = 0;
				var keyModification = new Dictionary<Vector2Int, Vector2Int>();
				
				foreach (var objectRender in _gameObjectList)
				{
					if (_parcelSeleced.ContainsKey(objectRender.Key))
						continue;
					if (parcelNeedObject.Count > 0)
					{
						keyModification.Add(objectRender.Key, parcelNeedObject[0]);
						UpdateBox(parcelNeedObject[0], _parcelSeleced[parcelNeedObject[0]], objectRender.Value);
						parcelNeedObject.RemoveAt(0);
					}
					else if (cachObject < 1000)
					{
						objectRender.Value.enabled = false;
						UpdateNeighbour(objectRender.Key);
						cachObject += 1;
					}
					else
					{
						Destroy(objectRender.Value.transform.parent.gameObject);
						UpdateNeighbour(objectRender.Key);
						keyModification.Add(objectRender.Key, new Vector2Int(-1, -1));
					}
				}

				foreach (var modification in keyModification)
				{
					if (modification.Value != new Vector2Int(-1, -1))
						_gameObjectList.Add(modification.Value, _gameObjectList[modification.Key]);
					_gameObjectList.Remove(modification.Key);
				}

				foreach (var parcel in parcelNeedObject)
				{
					_gameObjectList.Add(parcel,
						Instantiate(SelectBox, new Vector3(parcel.x, 0f, parcel.y), Quaternion.identity,
							SelectBoxParente).GetComponentInChildren<Renderer>());
					UpdateBox(parcel, _parcelSeleced[parcel], _gameObjectList[parcel]);
				}

				BoxSelectioCount = _gameObjectList.Count;
				//Debug.Log($"Update Box {_boxUpdated.Count}");
				_boxUpdated.Clear();
			}
		}

		public void UpdateBox(Vector2Int pos, Color color,  Renderer renderer, bool updateRecursion = true)
		{
			if (_boxUpdated.Contains(renderer))
				return;
			
			_boxUpdated.Add(renderer);
			
			var faceNeedRender = new bool[]
			{
				!_parcelSeleced.ContainsKey(pos + new Vector2Int(1, 0))||
				_parcelSeleced[pos + new Vector2Int(1, 0)] != color,
				
				!_parcelSeleced.ContainsKey(pos + new Vector2Int(0, -1))||
				_parcelSeleced[pos + new Vector2Int(0, -1)] != color,
				
				!_parcelSeleced.ContainsKey(pos + new Vector2Int(-1, 0)) ||
				_parcelSeleced[pos + new Vector2Int(-1, 0)] != color,
				
				!_parcelSeleced.ContainsKey(pos + new Vector2Int(0, 1)) ||
				_parcelSeleced[pos + new Vector2Int(0, 1)] != color,
			};
			
			var meshData = new MeshData();
			Parcel parcel = MapManager.map.GetParcel(pos);
			var cornerPos = new Vector3[]
			{
				new Vector3(1, parcel.corner[0] + 0.1f, 1),
				new Vector3(1, parcel.corner[1] + 0.1f, 0),
				new Vector3(0, parcel.corner[2] + 0.1f, 0),
				new Vector3(0, parcel.corner[3] + 0.1f, 1),
			};
			var cornerPosDown = new Vector3[4];
			
			for (int i = 0; i < 4; i++)
				cornerPosDown[i] = new Vector3(cornerPos[i].x, cornerPos[i].y - 1 ,cornerPos[i].z);
			
			meshData.AddTriangles(new Vector3[] { cornerPos[3], cornerPos[0], cornerPos[1] }, Vector2Int.zero);
			meshData.AddTriangles(new Vector3[] { cornerPos[3], cornerPos[1], cornerPos[2] }, Vector2Int.zero);

			for (int i = 0; i < 4; i++)
			{
				if (faceNeedRender[i])
				{
					meshData.AddTriangles(new Vector3[] { cornerPos[i], cornerPosDown[i], cornerPos[(i + 1) % 4] }, Vector2Int.zero);
					meshData.AddTriangles(new Vector3[] { cornerPos[(i + 1) % 4], cornerPosDown[i], cornerPosDown[(i + 1) % 4] }, Vector2Int.zero);
				}
			}

			var mesh = meshData.GetMesh();
			renderer.transform.parent.position = new Vector3(pos.x, 0, pos.y);
			renderer.material.color = color.ColorSetAlpha(0.7f);
			renderer.enabled = true;
			renderer.GetComponent<MeshFilter>().mesh = mesh;

			if (updateRecursion)
			{
				UpdateNeighbour(pos);
			}
		}

		private void UpdateNeighbour(Vector2Int pos)
		{
			foreach (var neighbour in MapManager.parcelAround)
			{
				if (_gameObjectList.ContainsKey(pos + neighbour) && _parcelSeleced.ContainsKey(pos + neighbour))
				{
					UpdateBox(pos + neighbour, _parcelSeleced[pos + neighbour], _gameObjectList[pos + neighbour], false);
				}
			}
		}
	}
}