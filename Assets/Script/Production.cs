using System;
using System.Collections.Generic;
using Script;
using Script.Game;
using UnityEngine;
using Random = UnityEngine.Random;


public class Production
{
	public ProductData data;

	public float quantity;
	public virtual int Quantity
	{
		get => Mathf.FloorToInt(quantity);
		set => quantity = value;
	}

	public float production;
	public float maxQuantity;
	public bool isInput;
	
	public Production(ProductData _data, float _max, bool _isInput, float _production)
	{
		data = _data;
		quantity = 0;
		maxQuantity = _max;
		isInput = _isInput;
		production = _production;
	}
	
	public bool IsOutput { get => !isInput;
		set => isInput = !value;
	}
	public float Filling => quantity / maxQuantity;

	public float AddQuantity(float value)
	{
		quantity += value;
		float quantityReturn = 0;
		if (quantity < 0)
		{
			quantityReturn += -quantity;
			quantity = 0;
		}

		if (quantity > maxQuantity)
		{
			quantityReturn += maxQuantity-quantity;
			quantity = maxQuantity;
		}
		return quantityReturn;
	}

	public void Update(float deltaTime)
	{
		if (isInput)
			AddQuantity(-production * deltaTime);
		else if (IsOutput)
			AddQuantity(production * deltaTime);
	}

	public static Production operator +(Production a, Production b)
	{
		if (a.data != b.data || a.isInput != b.isInput)
			throw new ArgumentException("Data is different");
		var productionInfo =
			new Production(a.data, a.maxQuantity + b.maxQuantity, a.isInput, a.production + b.production)
			{
				quantity = a.quantity + b.quantity
			};
		return productionInfo;
	}

	public void Add(Production add)
	{
		if (data != add.data || isInput != add.isInput)
			throw new ArgumentException("Production is different");

		quantity += add.quantity;
		maxQuantity += add.maxQuantity;
		production += add.production;

	}
	
	public static Production GetProduction(List<Production> list, ProductData data, bool isInput)
	{
		foreach (var production in list)
		{
			if (production.data == data && production.isInput == isInput)
				return production;
		}

		return null;
	}
}

public class ProductionCumulate
{
	private List<Production> productions;
	public bool isInput;
	public ProductData data;
	public int Quantity
	{
		get
		{
			int result = 0;
			foreach (var curProduction in productions)
				result += curProduction.Quantity;
			return result;
		}
	}
	public float MaxQuantity
	{
		get
		{
			float result = 0;
			foreach (var curProduction in productions)
				result += curProduction.maxQuantity;
			return result;
		}
	}
	
	public float Production
	{
		get
		{
			float result = 0;
			foreach (var curProduction in productions)
				result += curProduction.production;
			return result;
		}
	}
	
	public float Filling => Quantity / MaxQuantity;

	public ProductionCumulate(ProductData _data, bool _isInput)
	{
		isInput = _isInput;
		data = _data;
		productions = new List<Production>();
	}

	public void AddProduction(Production production)
	{
		if (production.data == data && production.isInput == isInput)
		{
			productions.Add(production);
			return;
		}

		throw new Exception("Type of prodution isn't equal");
	}
}