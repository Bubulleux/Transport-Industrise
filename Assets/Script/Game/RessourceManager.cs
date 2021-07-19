using System;
using UnityEngine;

namespace Script.Game
{
	public class RessourceManager : MonoBehaviour
	{
		public static RessourceManager instence;
		
		//Road Mesh
		public Mesh RoadStrainght;
		public Mesh RoadT;
		public Mesh RoadTurn;
		public Mesh RoadUphill;
		public Mesh RoadX;
		public Mesh RoadEnd;
		public Mesh RoadNoExit;
		public Material[] RoadMaterials;
		
		private void Start()
		{
			instence = this;
		}

	}
}