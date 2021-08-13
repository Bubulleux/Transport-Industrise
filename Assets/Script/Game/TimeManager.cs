using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private static TimeManager instance;
	public float timeScale = 1f;
	
	public static float TimeScale => instance.timeScale;
	public static float DeltaTime => Time.deltaTime * TimeScale;

	void Awake()
	{
		instance = this;
	}
	
	public void SetTimeScale(float _timeScale)
	{
		timeScale = _timeScale;
	}

}
