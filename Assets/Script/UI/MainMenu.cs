using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject loadGameMenu;
    public Transform loadGameMenuContente;

    public GameObject newGameMenu;
    public Transform newGameMenuContente;

    public void OpenNewGameMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(true);
    }

    public void OpenLoadGameMenu()
    {
        mainMenu.SetActive(false);
        loadGameMenu.SetActive(true);
        GameObject template = loadGameMenuContente.Find("Template").gameObject;
        foreach (Transform curChild in loadGameMenuContente)
        {
            if (curChild.gameObject.activeSelf == true)
            {
                Destroy(curChild.gameObject);
            }
        }
        foreach (string curSave in FIleSys.GetFolder("/Save"))
        {
            Transform curSaveObj = Instantiate(template).transform;
            curSaveObj.SetParent(template.transform.parent);
            curSaveObj.Find("SaveName").GetComponent<Text>().text = curSave;
            curSaveObj.Find("Load").GetComponent<Button>().onClick.AddListener(delegate { MapLoader.LoadSave(curSave); });
            curSaveObj.gameObject.SetActive(true);
        }
    }
    public void OpenSetting()
    {
        Debug.Log("Open Setting");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void Cancel()
    {
        mainMenu.SetActive(true);
        loadGameMenu.SetActive(false);
        newGameMenu.SetActive(false);
    }

    public void NewGame()
    {
        string saveName = newGameMenuContente.Find("SaveName").GetComponentInChildren<InputField>().text;
        MapLoader.GenerateMap(saveName);
    }
}
