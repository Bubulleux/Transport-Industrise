using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Game;
using Script.Mapping;
using Script.Mapping.ParcelType;
using UnityEngine;

public class RoadGFX : ParcelGFX
{
    public Mesh[] meshs;
    public MeshFilter meshFilter;

    private void Start()
    {
	    
    }

    public override void UpdateGFX()
    { 
	    var mesh = meshs[2];
	    
		var oneMaterial = false;
		transform.position = new Vector3(parcel.pos.x + 0.5f, parcel.corner.Max() + 0.01f, parcel.pos.y + 0.5f);

		var minCorner = parcel.corner.Min();
		var maxCorner = parcel.corner.Max();
		var connections = new bool[4];

		for (int i = 0; i < 4; i ++)
		{
			var neightbour = MapManager.parcelAround[i];
			connections[i] = parcel.map.ParcelIs<Road>(parcel.pos + neightbour) &&
			                 parcel.map.GetParcel<Road>(parcel.pos + neightbour).CanConnect(parcel.pos);

		}
		
		var connectionCount = 0;
		for (var i = 0; i < 4; i++)
		{
			if (connections[i])
				connectionCount++;
		}

		switch (connectionCount)
		{
			case 0:
				oneMaterial = true;
				mesh = meshs[1];
				break;
			case 1:
			{
				//oneMaterial = true;
				mesh = meshs[1];
				if (connections[0])
					transform.rotation = Quaternion.Euler(0, 270, 0);
				if (connections[1])
					transform.rotation = Quaternion.Euler(0, 0, 0);
				if (connections[2])
					transform.rotation = Quaternion.Euler(0, 90, 0);
				if (connections[3])
					transform.rotation = Quaternion.Euler(0, 180, 0);
				break;
			}
			case 2 when (connections[0] && !connections[1] && connections[2] && !connections[3]) || 
			            (!connections[0] && connections[1] && !connections[2] && connections[3]):
			{
				transform.rotation = Quaternion.Euler(0, connections[1] ? 0 : 90, 0);
				mesh = meshs[2];
				break;
			}
			case 2:
			{
				mesh = meshs[4];
				if (connections[0] && connections[1])
					transform.rotation = Quaternion.Euler(0, 270, 0);
				if (connections[1] && connections[2])
					transform.rotation = Quaternion.Euler(0, 0, 0);
				if (connections[2] && connections[3])
					transform.rotation = Quaternion.Euler(0, 90, 0);
				if (connections[3] && connections[0])
					transform.rotation = Quaternion.Euler(0, 180, 0);
				break;
			}
			case 3:
			{
				mesh = meshs[3];
				if (!connections[0])
					transform.rotation = Quaternion.Euler(0, 0, 0);
				if (!connections[1])
					transform.rotation = Quaternion.Euler(0, 90, 0);
				if (!connections[2])
					transform.rotation = Quaternion.Euler(0, 180, 0);
				if (!connections[3])
					transform.rotation = Quaternion.Euler(0, 270, 0);
				break;
			}
			case 4:
			{
				mesh = meshs[0];
				oneMaterial = true;
				break;
			}
		}

		meshFilter.sharedMesh = mesh;
		if (oneMaterial)
		{
			//MeshMaterial = new[] {RessourceManager.instence.RoadMaterials[0]};
		}
    }
}
