using System.Linq;
using Newtonsoft.Json;
using Script.Game;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	[JsonObject(MemberSerialization.OptOut)]
	public class Road : Parcel
	{

		public override void Initialaze()
		{
			color = Color.black;
			UpdateRoadObject();
		
		}

		public void UpdateRoadObject(bool debug = false)
		{
			ObjectMesh = RessourceManager.instence.RoadStrainght;
			MeshMaterial = RessourceManager.instence.RoadMaterials;
			var oneMaterial = false;
			ObjectPosition = new Vector3(0.5f, corner.Max() + 0.01f, 0.5f);
			var connections = new[]
			{
				map.ParcelIs<Road>(pos + new Vector2Int(1, 0)),
				map.ParcelIs<Road>(pos + new Vector2Int(0, 1)),
				map.ParcelIs<Road>(pos + new Vector2Int(-1, 0)),
				map.ParcelIs<Road>(pos + new Vector2Int(0, -1)),
			};
		
			var connectionCount = 0;
			for (var i = 0; i < 4; i++)
			{
				if (connections[i])
					connectionCount++;
			}
			if (debug)
			{
				Debug.Log($"Pos: {pos}, Connection  {connections[0]}, {connections[1]}, {connections[2]}, {connections[3]}, Connection Count {connectionCount}");
			}

			switch (connectionCount)
			{
				case 0:
					oneMaterial = true;
					ObjectMesh = RessourceManager.instence.RoadNoExit;
					break;
				case 1:
				{
					//oneMaterial = true;
					ObjectMesh = RessourceManager.instence.RoadEnd;
					if (connections[0])
						ObjectRotation = Quaternion.Euler(0, 270, 0);
					if (connections[1])
						ObjectRotation = Quaternion.Euler(0, 180, 0);
					if (connections[2])
						ObjectRotation = Quaternion.Euler(0, 90, 0);
					if (connections[3])
						ObjectRotation = Quaternion.Euler(0, 0, 0);
					break;
				}
				case 2 when (connections[0] && !connections[1] && connections[2] && !connections[3]) || 
				            (!connections[0] && connections[1] && !connections[2] && connections[3]):
				{
					if (debug) Debug.Log("Road Strainght");
					ObjectRotation = Quaternion.Euler(0, connections[1] ? 0 : 90, 0);
					ObjectMesh = RessourceManager.instence.RoadStrainght;
					break;
				}
				case 2:
				{
					if (debug) Debug.Log("Road Trune");
					ObjectMesh = RessourceManager.instence.RoadTurn;
					if (connections[2] && connections[3])
						ObjectRotation = Quaternion.Euler(0, 0, 0);
					if (connections[1] && connections[2])
						ObjectRotation = Quaternion.Euler(0, 90, 0);
					if (connections[0] && connections[1])
						ObjectRotation = Quaternion.Euler(0, 180, 0);
					if (connections[3] && connections[0])
						ObjectRotation = Quaternion.Euler(0, 270, 0);
					break;
				}
				case 3:
				{
					if (debug) Debug.Log("Road T");
					ObjectMesh = RessourceManager.instence.RoadT;
					if (!connections[0])
						ObjectRotation = Quaternion.Euler(0, 0, 0);
					if (!connections[1])
						ObjectRotation = Quaternion.Euler(0, 270, 0);
					if (!connections[2])
						ObjectRotation = Quaternion.Euler(0, 180, 0);
					if (!connections[3])
						ObjectRotation = Quaternion.Euler(0, 90, 0);
					break;
				}
				case 4:
				{
					if (debug) Debug.Log("Road X");
					ObjectMesh = RessourceManager.instence.RoadX;
					oneMaterial = true;
					break;
				}
			}

			if (oneMaterial)
			{
				MeshMaterial = new[] {RessourceManager.instence.RoadMaterials[0]};
			}
		}

		public override void DebugParcel()
		{
			UpdateRoadObject(true);
			//base.DebugParcel();
		}
	}
}
