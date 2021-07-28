using System.Collections.Generic;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	public class Dwelling : ParcelProducer
	{
		public int dwell;
		private BusStop busStop;
		private LoadingBay loadingBay;

		public override void InitializationSecondary()
		{
			color = Color.gray;
			dwell = Random.Range(2, 30);
			productions = new List<Production>
			{
				new Production(Resources.Load<ProductData>("ScriptableObject/Products/People"), dwell,
					false, dwell * 0.01f),
				new Production(Resources.Load<ProductData>("ScriptableObject/Products/Waste"), dwell * 5,
					false, dwell * 0.05f),
				new Production(Resources.Load<ProductData>("ScriptableObject/Products/Food"), dwell * 5,
					true, dwell * 0.025f)
			};
		}

		public override void DebugParcel()
		{
			base.DebugParcel();
			foreach (var production in productions)
			{
				Debug.Log($"{production.data} {production.quantity} {production.maxQuantity}");
			}
		}
	}
}
