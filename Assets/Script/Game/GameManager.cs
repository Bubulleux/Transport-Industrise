using UnityEngine;

namespace Script.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instence;
        public static string saveName;

        [SerializeField]
        private long money = 0;
        public static long Money 
        {
            get { return instence.money; } 
            set 
            {
                instence.money = value;
            } 
        }

        void Awake()
        {
            instence = this;
            money = 100000;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
