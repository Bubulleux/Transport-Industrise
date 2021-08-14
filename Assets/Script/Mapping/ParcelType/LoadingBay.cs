using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Script.Game;
using Script.UI.Windows;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	[JsonObject(MemberSerialization.OptOut)]
	public partial class LoadingBay : Road, ITradeProduct
	{

		protected int radiusAction = 15;
		
		public List<ParcelProducer> ProducerLink
		{
			get
			{
				List<ParcelProducer> returnList = new List<ParcelProducer>();
				foreach (Vector2Int circlePos in Helper.GetCircleArea(pos, radiusAction))
				{
					if (MapManager.map.ParcelIs<ParcelProducer>(circlePos))
					{
						returnList.Add(MapManager.map.GetParcel<ParcelProducer>(circlePos));
					}
				}
				return returnList;
			}
		}

		public List<ProductionCumulate> productionCumulates = new List<ProductionCumulate>();

		
		public override void InitializationSecondary()
		{
			color = Color.white;
			prefab = Resources.Load<GameObject>("ParcelGFX/LoadingBay");

			foreach (var producer in ProducerLink)
			{
				foreach (var production in producer.productions)
				{
					bool productionFind = false;
					foreach (var productionCumulate in productionCumulates)
					{
						if (productionCumulate.data == production.data &&
						    productionCumulate.isInput == production.isInput)
						{
							productionCumulate.AddProduction(production);
							productionFind = true;
							break;
						}
					}

					if (!productionFind)
					{
						var newProduction = new ProductionCumulate(production.data, production.isInput);
						productionCumulates.Add(newProduction);
						newProduction.AddProduction(production);
						
					}
				}
			}
		}

		public override void UpdateRoadObject(bool debug = false)  {  }

		public override void DebugParcel()
		{
			base.DebugParcel();
			Debug.Log($"Production Linked: {ProducerLink.Count}");
			string result = "Inpute:";
			foreach (var curProduction in GetProductions(true))
			{
				result += $"\n{curProduction.data}: {curProduction.Quantity} {curProduction.quantity} " + 
						  $"{curProduction.maxQuantity} {curProduction.Filling}";
			}
			result += "\nOutpute: ";
			foreach (var curProduction in GetProductions(false))
			{
				result += $"\n{curProduction.data}: {curProduction.Quantity} {curProduction.quantity} " +
						  $"{curProduction.maxQuantity} {curProduction.Filling}";
			}
			Debug.Log(result);

		}

		public override void Interact()
		{
			WindowsOpener.OpenLoadingBay(this);
		}

		public List<Production> GetProductions(bool getInput)
		{
			List<Production> productions = new List<Production>();
			foreach (var curProduction in GetProductions())
			{
				if (curProduction.isInput == getInput)
					productions.Add(curProduction);
			}
			return productions;
		}
		

		public List<Production> GetProductions()
		{
			List<Production> productions = new List<Production>();
			foreach (ParcelProducer curProducer in ProducerLink)
			{
				foreach (var curProduction in curProducer.productions)
				{
					if (Production.GetProduction(productions, curProduction.data, curProduction.isInput) == null)
					{
						productions.Add(new Production(curProduction.data, 0, curProduction.isInput, 0));
					}

					Production.GetProduction(productions, curProduction.data, curProduction.isInput).Add(curProduction);
				}
			}

			return productions;
		}

		
		public bool CanUnload(ProductData product)
		{
			foreach (ParcelProducer curIndustrise in ProducerLink)
			{
				foreach (var curMaterial in curIndustrise.productions)
				{
					if (curMaterial.isInput && curMaterial.data == product)
					{
						return true;
					}
				}
			}
			return false;
		}

		public int TryToInteract(ProductData product, int materialQuantityGive)
		{
			int materialHasNotGiven = materialQuantityGive;
			//Debug.Log("material has no Given" + materialHasNotGiven);
			foreach (ParcelProducer curIndustrise in ProducerLink)
			{
				if (materialHasNotGiven == 0)
					break;

				for (int i = 0; i < curIndustrise.productions.Count; i ++)
				{
					var curProduction = curIndustrise.productions[i];
					if (((curProduction.isInput && materialHasNotGiven > 0) ||
						 (curProduction.IsOutput && materialHasNotGiven < 0)) &&
						curProduction.data == product)
					{
						materialQuantityGive = Mathf.FloorToInt(-curProduction.AddQuantity(materialHasNotGiven));
					}

					curIndustrise.productions[i] = curProduction;
				}
			}
			int materialGive = materialHasNotGiven - materialQuantityGive;
			GameManager.Money += materialGive * (materialGive < 0 ? product.buyPrice : product.sellPrice);
			//Debug.Log("Materal Return " + materialGive);
			return materialGive;
		}

		public override bool CanConnect(Vector2Int connectionPos)
		{
			return Array.IndexOf(MapManager.parcelAround, connectionPos - pos) == (int)orientation;
		}
	}
}
