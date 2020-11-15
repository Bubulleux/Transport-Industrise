using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    private Task<Map> operation;
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
        operation = AsyncLoadMap(GameObject.Find("Map").GetComponent<MapManager>().heightCurv, GameObject.Find("Map").GetComponent<MapManager>().limitWaterCurv);
        
    }

    public async Task<Map> AsyncLoadMap(AnimationCurve heightCurv, AnimationCurve limitWaterCurv)
    {
        float startTime = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60;
        SceneManager.LoadSceneAsync(1);
        Map _mapdata = new Map();
        await _mapdata.GenerateMap(heightCurv, limitWaterCurv);
        //await Task.Delay(3000);
        SceneManager.LoadSceneAsync(0);
        Debug.Log(_mapdata.citys.Count);
        MapManager.map = _mapdata;
        //operation = null;
        Debug.Log("Load End");
        Destroy(gameObject);
        Debug.Log($"time to creat Map: {System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 - startTime}");
        return _mapdata;
    }
}
