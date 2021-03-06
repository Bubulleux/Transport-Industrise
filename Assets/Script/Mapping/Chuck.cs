using System;
using System.Collections.Generic;
using Script.Controler;
using Script.MapGeneration;
using Script.Mapping.ParcelType;
using UnityEngine;

namespace Script.Mapping
{
	public class Chuck : MonoBehaviour
	{
		public Vector2Int Pos;
		public Renderer RendererComponente;
		public MeshFilter MeshFilterComponente;
		public MeshCollider ColliderComponente;

		public bool NeedMeshUpdate = false;
		public bool NeedTextureUpdate = false;
		public bool NeedObjectUpdate = false;
		private static GameObject ChuckObjectPrefab;

		private Dictionary<Vector2Int, ChuckObject> Objects = new Dictionary<Vector2Int, ChuckObject>();

		private bool _chuckActive;

		public float camDist;

		public void Initialize(Vector2Int pos, Mesh mesh, Texture2D texture)
		{
			Pos = pos;
			if (!ChuckObjectPrefab)
				ChuckObjectPrefab = Resources.Load<GameObject>("ChuckObject");
		
			MeshFilterComponente.mesh = mesh;
			ColliderComponente.sharedMesh = mesh;
			// RendererComponente.sharedMaterial = new Material(Shader.Find("Standard"))
			// {
			// 	mainTexture = texture,
			// };
			RendererComponente.material.mainTexture = texture;
			_chuckActive = true;
			UpdateObject();
		}


		private void Update()
		{
			camDist = Vector3.Distance(new Vector3((Pos.x + 0.5f) * Map.ChuckSize, 0f, (Pos.y + 0.5f) * Map.ChuckSize),
				CamControler.camPos);
			
			if (camDist > 250 && _chuckActive)
			{
				
				SetChuckActive(false);
			}
			if (camDist < 250 && !_chuckActive)
			{
				SetChuckActive(true);
			}
		}

		private void SetChuckActive(bool enable)
		{
			_chuckActive = enable;
			foreach (var obj in Objects)
			{
				obj.Value.RendererComponente.enabled = enable;
			}
		}
		
		public void UpdateChuck(bool force)
		{
			if (NeedMeshUpdate || force)
			{
				UpdateMesh();
				NeedMeshUpdate = false;
			}
		
			if (NeedTextureUpdate || force)
			{
				UpdateTexture();
				NeedTextureUpdate = false;
			}
		
			if (NeedObjectUpdate || force)
			{
				UpdateObject();
				NeedObjectUpdate = false;
			}
		}
	
		private void UpdateTexture()
		{
			RendererComponente.sharedMaterial.mainTexture = TextureGenerator.GetChunkTexture(Pos, MapManager.map);
		}

		private void UpdateMesh()
		{
			var mesh = MeshGenerator.GetChunkMesh(Pos, MapManager.map);
			MeshFilterComponente.mesh = mesh;
			ColliderComponente.sharedMesh = mesh;
		}

		private void UpdateObject()
		{
			for (var y = 0; y < Map.ChuckSize; y++)
			{
				for (var x = 0; x < Map.ChuckSize; x++)
				{
					Vector2Int curPos = new Vector2Int(x, y);
					Parcel parcel = MapManager.map.GetParcel(curPos + Pos * Map.ChuckSize);

					if (parcel.ObjectMesh && !Objects.ContainsKey(curPos))
					{
						var go = Instantiate(ChuckObjectPrefab, parcel.ObjectPosition, parcel.ObjectRotation, transform);
						Objects.Add(curPos, new ChuckObject()
						{
							GO = go,
							RendererComponente = go.GetComponent<Renderer>(),
							MeshComponente = go.GetComponent<MeshFilter>(),
						});
					}
				
					if (!parcel.ObjectMesh && Objects.ContainsKey(curPos))
					{
						Destroy(Objects[curPos].GO);
						Objects.Remove(curPos);
					}

					if (parcel.ObjectMesh && Objects.ContainsKey(curPos))
					{
						var curObject = Objects[curPos];
						curObject.GO.transform.position = parcel.ObjectPosition +  new Vector3(curPos.x + Pos.x * Map.ChuckSize, 0f, curPos.y + Pos.y * Map.ChuckSize);
						curObject.GO.transform.rotation = parcel.ObjectRotation;

						curObject.RendererComponente.materials = parcel.MeshMaterial;
						curObject.MeshComponente.sharedMesh = parcel.ObjectMesh;
						
						curObject.RendererComponente.enabled = _chuckActive;
					}
				}
			}
		}

		

		private struct ChuckObject
		{
			public GameObject GO;
			public Renderer RendererComponente;
			public MeshFilter MeshComponente;
		}
	}
}
