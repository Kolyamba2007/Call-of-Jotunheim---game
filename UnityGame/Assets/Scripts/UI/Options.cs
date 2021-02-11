using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour
{   
    // СТАНДАРТНЫЕ ЗНАЧЕНИЯ ОПЦИЙ
    private static KeyCode DefaultRight { set; get; } = KeyCode.D;
    private static KeyCode DefaultLeft { set; get; } = KeyCode.A;
    private static KeyCode DefaultJump { set; get; } = KeyCode.Space;
    private static KeyCode DefaultInteraction { set; get; } = KeyCode.E;
    private static KeyCode DefaultMeleeAttack { set; get; } = KeyCode.Mouse0;
    private static KeyCode DefaultRangeAttack { set; get; } = KeyCode.LeftShift;
    private static KeyCode DefaultGameMenu { set; get; } = KeyCode.Escape;
    private static KeyCode DefaultDeveloperPanel { set; get; } = KeyCode.BackQuote;

    private static bool DefaultTipsOn { set; get; } = true;
    private static float DefaultCameraAttachedDepth { set; get; } = 0.5f;
    private static float DefaultCameraAttachedSpeed { set; get; } = 0.8f;
    private static float DefaultCameraAttachedThreshold { set; get; } = 0.3f;
    private static float DefaultCameraDamping { set; get; } = 1.5f;

    // ТЕКУЩИЕ ЗНАЧЕНИЯ ОПЦИЙ
    public static KeyCode Right { set; get; } = DefaultRight;
    public static KeyCode Left { set; get; } = DefaultLeft;
    public static KeyCode Jump { set; get; } = DefaultJump; 
    public static KeyCode Interaction { set; get; } = DefaultInteraction;
    public static KeyCode MeleeAttack { set; get; } = DefaultMeleeAttack;
    public static KeyCode RangeAttack { set; get; } = DefaultRangeAttack;
    public static KeyCode GameMenu { set; get; } = DefaultGameMenu;
    public static KeyCode DeveloperPanel { set; get; } = DefaultDeveloperPanel;

    public static bool TipsOn { set; get; } = DefaultTipsOn;
    public static float CameraAttachedDepth { set; get; } = DefaultCameraAttachedDepth;
    public static float CameraAttachedSpeed { set; get; } = DefaultCameraAttachedSpeed;
    public static float CameraAttachedThreshold { set; get; } = DefaultCameraAttachedThreshold;
    public static float CameraDamping { set; get; } = DefaultCameraDamping;
    public static Vector2 CameraAttachedOffset { private set; get; } = new Vector2(2f, 1f);

    /* ------------------------------------ */
    public static bool IsKeyBeingChanging { private set; get; } = false;
    private static GameObject changedButton;
    private static KeyCode changedKey = KeyCode.None;
    private static ColorBlock defaultColorBlock;
    
    [Header("Виды опций: Игра, Управление, Звук")]
    public GameObject[] OptionsType;

    [Header("Настройки игры")]
    public GameObject[] GameOptions;

    [Header("Настройки управления")]
    public GameObject [] PlayerControlOptions;

    [Header("Настройки звука")]
    public GameObject[] SoundOptions;

    private static GameObject currentOption;

    private void Awake()
    {
        defaultColorBlock.normalColor = Color.white;
        defaultColorBlock.highlightedColor = Color.white;
        defaultColorBlock.pressedColor = Color.grey;
        defaultColorBlock.selectedColor = Color.white;
        defaultColorBlock.colorMultiplier = 1;

        Reset();
    }
    private void OnGUI()
    {
        if (Input.anyKeyDown && Event.current.keyCode != KeyCode.None)
        {
            changedKey = Event.current.keyCode;

            if (IsKeyBeingChanging)
            {
                switch (changedButton.name)
                {
                    case "Right Button":
                        Right = changedKey;
                        break;
                    case "Left Button":
                        Left = changedKey;
                        break;
                    case "Jump Button":
                        Jump = changedKey;
                        break;                    
                    case "Interaction Button":
                        Interaction = changedKey;
                        break;
                    case "Melee Attack Button":
                        MeleeAttack = changedKey;
                        break;
                    case "Range Attack Button":
                        RangeAttack = changedKey;
                        break;
                    case "Game Menu Button":
                        GameMenu = changedKey;
                        break;
                    case "Developer's Panel Button":
                        DeveloperPanel = changedKey;
                        break;
                }
                changedButton.GetComponentInChildren<Text>().text = changedKey.ToString();

                ChangeKeyOff();
            }
        }
    }

    public void SwitchOption(GameObject _option)
    { 
        if (currentOption != _option)
        {
            if (currentOption != null) currentOption.GetComponent<Button>().interactable = true;

            currentOption = _option;
            currentOption.GetComponent<Button>().Select();
            currentOption.GetComponent<Button>().interactable = false;
            foreach (GameObject option in OptionsType)
            {
                if (option.name != currentOption.name) option.SetActive(false);
                else option.SetActive(true);
            }
        }
    } // Выбор опции
    public void ChangeKey(GameObject _button)
    {
        if (IsKeyBeingChanging && changedButton == _button)
        {
            ChangeKeyOff();
        }
        else
        {
            changedButton = _button;

            for (int i = 0; i < PlayerControlOptions.Length; i++)
            {
                if (PlayerControlOptions[i] != _button) PlayerControlOptions[i].GetComponent<Button>().colors = defaultColorBlock;
            }

            ColorBlock _colorBlock = defaultColorBlock;
            _colorBlock.highlightedColor = Color.blue;
            _colorBlock.normalColor = Color.blue;

            _button.GetComponent<Button>().colors = _colorBlock;
            IsKeyBeingChanging = true;
        }
    } // Смена клавиши
    public void ChangeKeyOff()
    {
        changedKey = KeyCode.None;
        if (changedButton != null)
        {
            changedButton.GetComponent<Button>().colors = defaultColorBlock;
            changedButton = null;
        }
        IsKeyBeingChanging = false;
    }  // Сброс режима смены клавиши

    public void SetCameraAttachedSpeed(GameObject _slider)
    {
        CameraAttachedSpeed = _slider.GetComponent<Slider>().value;
    }

    public void SetCameraAttachedThreshold(GameObject _scrollbar)
    {
        CameraAttachedThreshold = _scrollbar.GetComponentInChildren<Scrollbar>().value;
    }

    public void ChangeTipsOn()
    {
        //TipsOn = OptionsParameters[7].GetComponent<Toggle>().isOn;
    }
    public void Reset()
    {
        // СБРОС КЛАВИШ          
        Right = DefaultRight;
        Left = DefaultLeft;
        Jump = DefaultJump;
        Interaction  = DefaultInteraction;
        MeleeAttack = DefaultMeleeAttack;
        RangeAttack = DefaultRangeAttack;
        GameMenu = DefaultGameMenu;
        DeveloperPanel = DefaultDeveloperPanel;

        PlayerControlOptions[0].GetComponentInChildren<Text>().text = DefaultRight.ToString();
        PlayerControlOptions[1].GetComponentInChildren<Text>().text = DefaultLeft.ToString();
        PlayerControlOptions[2].GetComponentInChildren<Text>().text = DefaultJump.ToString();
        PlayerControlOptions[3].GetComponentInChildren<Text>().text = DefaultInteraction.ToString();
        PlayerControlOptions[4].GetComponentInChildren<Text>().text = DefaultMeleeAttack.ToString();
        PlayerControlOptions[5].GetComponentInChildren<Text>().text = DefaultRangeAttack.ToString();
        PlayerControlOptions[6].GetComponentInChildren<Text>().text = DefaultGameMenu.ToString();
        PlayerControlOptions[7].GetComponentInChildren<Text>().text = DefaultDeveloperPanel.ToString();       
            

        // СБРОС ИГРОВЫХ НАСТРОЕК        
        CameraAttachedSpeed = DefaultCameraAttachedSpeed;
        CameraAttachedDepth = DefaultCameraAttachedDepth;
        CameraAttachedThreshold = DefaultCameraAttachedThreshold;
        CameraDamping = DefaultCameraDamping;

        GameOptions[0].GetComponentInChildren<Slider>().value = DefaultCameraAttachedSpeed;
        GameOptions[1].GetComponentInChildren<Scrollbar>().value = DefaultCameraAttachedThreshold;
    } // Сброс настроек
}
