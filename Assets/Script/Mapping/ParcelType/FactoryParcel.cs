using System.Collections.Generic;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	public class FactoryParcel : ParcelProducer
	{
		public IndustriseData data;
		public static int maxMaterialCanStock = 300;

		public FactoryParcel(IndustriseData _data)
		{
			data = _data;
		}
		public override void InitializationSecondary()
		{
			if (!data)
				return;
			color = data.color;
			productions = new List<Production>();
			
			if (data.productIn != null)
				foreach (var product in data.productIn)
					productions.Add(new Production(product, maxMaterialCanStock, true, Random.Range(2f, 30f)));
			
			
			if (data.productOut != null)
				foreach (var product in data.productOut)
					productions.Add(new Production(product, maxMaterialCanStock, false, Random.Range(2f, 30f)));
		}
	}
}