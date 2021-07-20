using UnityEngine;

namespace Script.UI.Button
{
    public class ButtonInteractMoney : MonoBehaviour
    {
        public delegate bool Condiction();
        public Condiction condiction;
        void Update()
        {
            GetComponent<UnityEngine.UI.Button>().interactable = condiction();
        }
    }
}
