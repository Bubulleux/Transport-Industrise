using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
