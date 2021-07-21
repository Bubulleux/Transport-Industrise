using Script.Controler;
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
        public Dropdown ToolMenu;
        public Dropdown ToolMode;
        void Start()
        {
            foreach (var tool in PlayerControler.instance.Tools)
            {
                ToolMenu.options.Add(new Dropdown.OptionData(tool.Name));
            }
            SetTool(0);
        }

        public void SetTool(int tool)
        {
            PlayerControler.instance.curTool = PlayerControler.instance.Tools[tool];
            ToolMode.ClearOptions();
            if (PlayerControler.instance.curTool.Modes != null)
            {
                foreach (var mode in PlayerControler.instance.curTool.Modes)
                {
                    ToolMode.options.Add(new Dropdown.OptionData(mode));
                }
                ToolMode.interactable = true;
                ToolMode.value = PlayerControler.instance.curTool.modeUsed;
            }
            else
                ToolMode.interactable = false;

            ToolMode.value = PlayerControler.instance.curTool.modeUsed;
            ToolMode.RefreshShownValue();
        }

        public void SetMode(int mode)
        {
            PlayerControler.instance.curTool.modeUsed = mode;
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
