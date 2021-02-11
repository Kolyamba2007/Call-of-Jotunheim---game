using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wilberforce.FinalVignette;

public class Interface : MonoBehaviour
{
    public GameObject InterfacePanel;
    public GameObject DefeatPanel;
    public GameObject OptionsPanel;
    public GameObject GameMenuPanel;
    public GameObject BossBar;
    public GameObject DeveloperPanel;

    public static GameObject CurrentPanel { set; get; } = null;


    public static bool Hidden { private set; get; } = false;

    private void Update()
    {
        if (Input.GetKeyDown(Options.GameMenu))
        {
            if (!Game.IsDefeat && !GameMenuPanel.gameObject.activeInHierarchy)
            {
                Game.Pause(true);
                Open(GameMenuPanel);
                GameMenuPanel.GetComponent<GameMenu>().Open();
            }
            else
            {
                Close();
            }
        }

        if (Input.GetKeyDown(Options.DeveloperPanel))
        {
            if (!DeveloperPanel.activeInHierarchy)
            {
                Open(DeveloperPanel);
                Player.Hero.Commandable = false;
            }
            else
            {
                Close();
                DeveloperPanel.transform.GetChild(1).GetComponent<InputField>().text = "";
                Player.Hero.Commandable = true;
            }
        }
    }

    // ФУНКЦИИ ПОЛЬЗОВАТЕЛЬСКОГО ИНТРЕФЕЙСА
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public static void Show(bool _status)
    {
        GameObject.Find("Canvas").GetComponent<Interface>().InterfacePanel.SetActive(_status);
        Hidden = !_status;
    } // Скрыть пользовательский интерфейс
    public void Open(GameObject _panel)
    {
        _panel.transform.SetAsLastSibling();
        _panel.SetActive(true);
        CurrentPanel = GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).gameObject;
    } // Открыть указанное окно интерфейса
    public void Close(GameObject _panel)
    {
        if (_panel == GameMenuPanel)
        {
            Game.Pause(false);
            GameMenuPanel.GetComponent<Animator>().SetTrigger("Close");
        }

        _panel.transform.SetAsFirstSibling();
        _panel.SetActive(false);
        CurrentPanel = GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).gameObject;
    }  // Закрыть указанное окно интерфейса
    public void Close()
    {
        if (CurrentPanel != DefeatPanel)
        {
            if (CurrentPanel == GameMenuPanel)
            {
                Game.Pause(false);
                GameMenuPanel.GetComponent<GameMenu>().Close();
            }
            else
            {
                CurrentPanel.transform.SetAsFirstSibling();
                CurrentPanel.SetActive(false);
                CurrentPanel = GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).gameObject;
            }
        }
    } // Закрыть последнее окно интерфейса    
}
