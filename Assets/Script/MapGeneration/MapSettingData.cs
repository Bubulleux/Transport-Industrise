using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSetting", menuName = "MyGame/MapSetting")]
public class MapSettingData : ScriptableObject
{
    public Vector2Int chunkCount;
    public AnimationCurve heightCurv;
    public AnimationCurve limitWaterCurv;
}
