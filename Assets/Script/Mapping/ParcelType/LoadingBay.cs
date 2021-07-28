using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Script.Game;
using Script.UI.Windows;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	[JsonObject(MemberSerialization.OptOut)]
	public partial class LoadingBay : Road
	{
		[JsonIgnore]
		public List<ParcelProducer> ProductionLink
		{
			get
			{
				List<ParcelProducer> returnList = new List<ParcelProducer>();
				foreach (Vector2Int circlePos in Helper.GetCircleArea(pos, 30))
				{
					if (MapManager.map.ParcelIs<ParcelProducer>(circlePos))
					{
						returnList.Add(MapManager.map.GetParcel<ParcelProducer>(circlePos));
					}
				}
				return returnList;
			}
		}

		
		public override void InitializationSecondary()
		{
			color = Color.white;
			prefab = Resources.Load<GameObject>("ParcelGFX/LoadingBay");
		}

		public override void UpdateRoadObject(bool debug = false)  {  }

		public override void DebugParcel()
		{
			base.DebugParcel();
			Debug.Log($"Production Linked: {ProductionLink.Count}");
			string result = "Inpute:";
			foreach (KeyValuePair<ProductData, Production> curProduction in GetProductions(true))
			{
				result += $"\n{curProduction.Key}: {curProduction.Value.Quantity} {curProduction.Value.quantity} " + 
						  $"{curProduction.Value.maxQuantity} {curProduction.Value.Filling}";
			}
			result += "\nOutpute: ";
			foreach (KeyValuePair<ProductData, Production> curProduction in GetProductions(false))
			{
				result += $"\n{curProduction.Key}: {curProduction.Value.Quantity} {curProduction.Value.quantity} " +
						  $"{curProduction.Value.maxQuantity} {curProduction.Value.Filling}";
			}
			Debug.Log(result);

		}

		public override void Interact()
		{
			base.Interact();
			WindowsOpener.OpenLoadingBay(this);
		}

		public virtual Dictionary<ProductData, Production> GetProductions(bool getInput)
		{
			Dictionary<ProductData, Production> resulte = new Dictionary<ProductData, Production>();
			foreach (ParcelProducer curIndustrise in ProductionLink)
			{
				foreach (var curProduction in curIndustrise.productions)
				{
					if (curProduction.isInput != getInput)
						continue;
					if (resulte.ContainsKey(curProduction.data))
					{
						resulte[curProduction.data] += curProduction;
					}
					else
					{
						resulte.Add(curProduction.data, curProduction);
					}

					if (curProduction.quantity != 0 && curProduction.quantity != curProduction.maxQuantity)
					{
						Debug.Log($"Error {curIndustrise.pos} {curProduction.data} {curProduction.quantity} {curProduction.maxQuantity}");
					}
				}
			}

			return resulte;
		}
		
		public virtual bool CanUnload(ProductData product)
		{
			foreach (ParcelProducer curIndustrise in ProductionLink)
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

		public virtual int TryToInteract(ProductData product, int materialQuantityGive)
		{
			int materialHasNotGiven = materialQuantityGive;
			//Debug.Log("material has no Given" + materialHasNotGiven);
			foreach (ParcelProducer curIndustrise in ProductionLink)
			{
				if (materialHasNotGiven == 0)
				{
					break;
				}

				for (int i = 0; i < curIndustrise.productions.Count; i ++)
				{
					var curMaterial = curIndustrise.productions[i];
					if (((curMaterial.isInput && materialHasNotGiven > 0) ||
						 (curMaterial.IsOutput && materialHasNotGiven < 0)) &&
						curMaterial.data == product)
					{
						materialQuantityGive = Mathf.FloorToInt(-curMaterial.AddQuantity(materialHasNotGiven));
					}

					curIndustrise.productions[i] = curMaterial;
				}
			}
			int materialGive = materialQuantityGive - materialHasNotGiven;
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
