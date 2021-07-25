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
			base.Initialaze();
			foreach (var parcel in MapManager.parcelAround)
			{
				if (map.ParcelIs<Road>(parcel + pos))
					map.GetParcel<Road>(parcel + pos).UpdateRoadObject();
			}
		}

		public override void InitializationSecondary()
		{
			color = new Color(0.06f, 0.06f, 0.06f);
			prefab = Resources.Load<GameObject>("ParcelGFX/Road");
			UpdateRoadObject();
		}

		public virtual void UpdateRoadObject(bool debug = false)
		{
			if (gfx == null)
			{
				return;
			}
			gfx.GetComponent<RoadGFX>().UpdateGFX();
		}


		public virtual bool CanConnect(Vector2Int connectionPos)
		{
			return true;
		}
		

		public override void DebugParcel()
		{
			UpdateRoadObject(true);
			//base.DebugParcel();
		}
	}
}
