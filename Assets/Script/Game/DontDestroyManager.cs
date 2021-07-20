using UnityEngine;

namespace Script.Game
{
    public class DontDestroyManager : MonoBehaviour
    {
        static DontDestroyManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
