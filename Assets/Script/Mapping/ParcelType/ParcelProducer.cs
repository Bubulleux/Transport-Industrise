using System.Collections.Generic;
using Script.Game;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	public class ParcelProducer : Parcel
	{
		public List<Production> productions;

		public void UpdateProduction(float deltaTime)
		{
			for (int i = 0; i < productions.Count; i ++)
			{
				productions[i].Update(deltaTime);
			}
		}
		
	}
}