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
			color = new Color(0.06f, 0.06f, 0.06f);
			prefab = Resources.Load<GameObject>("ParcelGFX/Road");
			UpdateRoadObject();
		}

		public void UpdateRoadObject(bool debug = false)
		{
			if (gfx == null)
			{
				return;
			}
			gfx.GetComponent<RoadGFX>().UpdateGFX();
		}

		

		public override void DebugParcel()
		{
			UpdateRoadObject(true);
			//base.DebugParcel();
		}
	}
}
