using System;
using System.Collections.Generic;
using Script;
using Script.Game;
using UnityEngine;
using Random = UnityEngine.Random;


public class Production
{
	public Production(ProductData _data, float _max, bool _isInput, float _production)
	{
		data = _data;
		quantity = 0;
		maxQuantity = _max;
		isInput = _isInput;
		production = _production;
	}

	public ProductData data;

	public float quantity;
	public int Quantity
	{
		get => Mathf.FloorToInt(quantity);
		set => quantity = value;
	}

	public float production;
	public float maxQuantity;
	public bool isInput;
		
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