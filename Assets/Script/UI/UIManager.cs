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
            foreach (var tool in PlayerControler.instance.tools)
            {
                ToolMenu.options.Add(new Dropdown.OptionData(tool.Name));
            }
            SetTool(0);
        }

        public void SetTool(int tool)
        {
            PlayerControler.instance.CurTool = PlayerControler.instance.tools[tool];
            ToolMode.ClearOptions();
            if (PlayerControler.instance.CurTool.Modes != null)
            {
                foreach (var mode in PlayerControler.instance.CurTool.Modes)
                {
                    ToolMode.options.Add(new Dropdown.OptionData(mode));
                }
                ToolMode.interactable = true;
                ToolMode.value = PlayerControler.instance.CurTool.modeUsed;
            }
            else
                ToolMode.interactable = false;

            ToolMode.value = PlayerControler.instance.CurTool.modeUsed;
            ToolMode.RefreshShownValue();
        }

        public void SetMode(int mode)
        {
            PlayerControler.instance.CurTool.modeUsed = mode;
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
