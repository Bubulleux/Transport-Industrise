using UnityEngine;

namespace Script.Mapping.ParcelType
{
	public class Dwelling : Parcel
	{
		public int dwell;
		private BusStop busStop;
		public override void Initialaze()
		{
			color = Color.gray;
			dwell = Random.Range(3, 20);
		}
	}
}
