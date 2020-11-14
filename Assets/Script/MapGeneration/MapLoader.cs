using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    private Task<MapData> operation;
    public static bool load = false;
    public void Awake()
    {
        Debug.Log(load);
        if (load == true)
        {
            Destroy(gameObject);
            return;
        }
        load = true;
        Debug.Log("Load");
        DontDestroyOnLoad(gameObject);
        operation = AsyncLoadMap(GameObject.Find("Map").GetComponent<Map>().heightCurv, GameObject.Find("Map").GetComponent<Map>().limitWaterCurv);
        
    }

    public async Task<MapData> AsyncLoadMap(AnimationCurve heightCurv, AnimationCurve limitWaterCurv)
    {
        SceneManager.LoadSceneAsync(1);
        MapData _mapdata = new MapData();
        await _mapdata.GenerateMap(heightCurv, limitWaterCurv);
        //await Task.Delay(3000);
        SceneManager.LoadSceneAsync(0);
        Debug.Log(_mapdata.citys.Count);
        Map.mapData = _mapdata;
        //operation = null;
        Debug.Log("Load End");
        Destroy(gameObject);
        return _mapdata;
    }
}
