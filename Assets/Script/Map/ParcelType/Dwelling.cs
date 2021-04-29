using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwelling : Parcel
{
	public int dwell;
	public override void Initialaze()
	{
		color = Color.gray;
		dwell = Random.Range(3, 20);
	}
}
