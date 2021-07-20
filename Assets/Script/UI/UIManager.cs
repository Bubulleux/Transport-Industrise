using Script.Game;
using Script.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIManager : MonoBehaviour
    {

        // Start is called before the first frame update
        public Text moneyUi;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            moneyUi.text = "$ " + GameManager.Money;
        }

        public void OpenRoutesListWindow()
        {
            WindowsOpener.OpenRouteListWindow();
        }

        public void OpenGroupesListWindow()
        {
            WindowsOpener.OpenGroupesListWindow();
        }
    }
}
