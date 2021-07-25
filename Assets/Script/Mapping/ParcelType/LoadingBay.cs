using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Script.Game;
using Script.UI.Windows;
using UnityEngine;

namespace Script.Mapping.ParcelType
{
	[JsonObject(MemberSerialization.OptOut)]
	public class LoadingBay : Road
	{
		public LoadingBayType loadingBayType;
		public enum LoadingBayType
		{
			LoadingBay,
			BusStop
		}

		[JsonIgnore]
		public List<Industrise> IndustriseLink
		{
			get
			{
				List<Industrise> returnList = new List<Industrise>();
				foreach (Industrise curIndustrise in MapManager.map.industrises)
				{
					if (Vector2Int.Distance(pos, curIndustrise.MasterPos) <= 30)
					{
						returnList.Add(curIndustrise);
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
			string result = "Inpute:";
			foreach (KeyValuePair<MaterialData, MaterialInfo> curMaterial in GetMaterial(true))
			{
				result += $"\n{curMaterial.Key}: {curMaterial.Value.quantity}";
			}
			result += "\nOutpute: ";
			foreach (KeyValuePair<MaterialData, MaterialInfo> curMaterial in GetMaterial(false))
			{
				result += $"\n{curMaterial.Key}: {curMaterial.Value.quantity}";
			}
			Debug.Log(result);

		}

		public override void Interact()
		{
			base.Interact();
			WindowsOpener.OpenLoadingBay(this);
		}

		public virtual Dictionary<MaterialData, MaterialInfo> GetMaterial(bool getInput)
		{
			Dictionary<MaterialData, MaterialInfo> resulte = new Dictionary<MaterialData, MaterialInfo>();
			foreach (Industrise curIndustrise in IndustriseLink)
			{
				foreach (KeyValuePair<MaterialData, int> curMaterial in getInput ? curIndustrise.materialsInpute : curIndustrise.materialsOutpute)
				{
					if (resulte.ContainsKey(curMaterial.Key))
					{
						resulte[curMaterial.Key] = new MaterialInfo()
						{
							quantity = resulte[curMaterial.Key].quantity + curMaterial.Value,
							maxQuantity = resulte[curMaterial.Key].maxQuantity + Industrise.maxMaterialCanStock,
						};
					}
					else
					{
						resulte.Add(curMaterial.Key, new MaterialInfo() 
						{ 
							quantity = curMaterial.Value,
							maxQuantity = Industrise.maxMaterialCanStock,
							isInput = getInput,
						});
					}
				}
			}
			return resulte;
		}

		//public virtual int GiveOrTakeMaterial(MaterialData material, int quantity)
		//{
		//	int _quantity = quantity;
		//	foreach (Industrise curIndustrise in IndustriseLink)
		//	{
		//		if (curIndustrise.materialsInpute.ContainsKey(material))
		//		{
		//			curIndustrise.materialsInpute[material] += _quantity;
		//			_quantity = 0;
		//			if (curIndustrise.materialsInpute[material] - Industrise.maxMaterialCanStock > 0)
		//			{
		//				_quantity = curIndustrise.materialsInpute[material] - Industrise.maxMaterialCanStock;
		//				curIndustrise.materialsInpute[material] = Industrise.maxMaterialCanStock;
		//			}

		//			if (curIndustrise.materialsInpute[material] < 0)
		//			{
		//				quantity = curIndustrise.materialsInpute[material];
		//				curIndustrise.materialsInpute[material] = 0;
		//			}
		//		}
		//	}
		//	return _quantity;
		//}

		public virtual bool CanUnload(MaterialData material)
		{
			foreach (Industrise curIndustrise in IndustriseLink)
			{
				if (curIndustrise.materialsInpute.ContainsKey(material) && curIndustrise.materialsInpute[material] != Industrise.maxMaterialCanStock)
				{
					return true;
				}
			}
			return false;
		}

		public virtual int TryToInteract(MaterialData material, int materialQuantityGive)
		{
			int materialHasNotGiven = materialQuantityGive;
			//Debug.Log("material has no Given" + materialHasNotGiven);
			foreach (Industrise curIndustrise in IndustriseLink)
			{
				if (materialHasNotGiven == 0)
				{
					break;
				}
				if (materialHasNotGiven > 0 && curIndustrise.materialsInpute.ContainsKey(material) && curIndustrise.materialsInpute[material] != Industrise.maxMaterialCanStock)
				{
					curIndustrise.materialsInpute[material] += materialHasNotGiven;
					if (curIndustrise.materialsInpute[material] > Industrise.maxMaterialCanStock)
					{
						materialHasNotGiven = curIndustrise.materialsInpute[material] - Industrise.maxMaterialCanStock;
						curIndustrise.materialsInpute[material] = Industrise.maxMaterialCanStock;
					}
					else
					{
						materialHasNotGiven = 0;
					}
				}
				if (materialHasNotGiven < 0 && curIndustrise.materialsOutpute.ContainsKey(material) && curIndustrise.materialsOutpute[material] != 0)
				{
					curIndustrise.materialsOutpute[material] += materialHasNotGiven;
					if (curIndustrise.materialsOutpute[material] < 0)
					{
						materialHasNotGiven = curIndustrise.materialsOutpute[material];
						curIndustrise.materialsOutpute[material] = 0;
					}
					else
					{
						materialHasNotGiven = 0;
					}
				}

			}
			int materialGive = materialQuantityGive - materialHasNotGiven;
			GameManager.Money += materialGive * (materialGive < 0 ? material.buyPrice : material.sellPrice);
			//Debug.Log("Materal Return " + materialGive);
			return materialGive;
		}

		public struct MaterialInfo
		{
			public int quantity;
			public int maxQuantity;
			public bool isInput;
			public bool IsOutput { get { return !isInput; } set { isInput = !value; } }
			public float Filling { get { return quantity / (float)maxQuantity; } }
		}
		
		public override bool CanConnect(Vector2Int connectionPos)
		{
			return Array.IndexOf(MapManager.parcelAround, connectionPos - pos) == (int)orientation;
		}
	}
}
