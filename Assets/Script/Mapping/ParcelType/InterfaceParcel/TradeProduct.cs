using System.Collections.Generic;
using Script;
using UnityEngine;

public interface ITradeProduct
{

	public List<Production> GetProductions(bool getInput);
	public List<Production> GetProductions();
	public bool CanUnload(ProductData product);
	public int TryToInteract(ProductData product, int materialQuantityGive);

}

public interface ITradePeople
{
	public int GetPeople();
	public void Unload(int peopleCount);
	public List<Vector2Int> LoadPeople(int count, Route route);
}