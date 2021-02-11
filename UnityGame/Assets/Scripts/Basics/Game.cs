using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static bool IsDefeat { set; get; } = false;
    public static bool IsPaused { set; get; } = false;

    private void Start()
    {
        Initialize();
    }
    public static void Initialize()
    {
        IsDefeat = false;
        IsPaused = false;
        Player.Hero.Health = 100;
        Player.Hero.IsDead = false;
        Player.Hero.CanJump = true;
        Player.Hero.CanSlide = false;
        Player.Hero.Movable = true;
        Player.Hero.Commandable = true;
        Player.Hero.OnGround = true;
        Player.Hero.IsReadyToAttack = true;
        foreach (Unit unit in Unit.Units)
        {
            if (unit != null)
            {
                if (unit.tag == "Enemy")
                {
                    if (unit.GetComponent<Enemy>().OnPatrol) unit.Queue.Add(new Order(method => unit.GetComponent<Enemy>().Patrol(), "Патрулирование"));
                }
            }
        }

        Sound.Play(Sound.GameEventSoundType.LOCATION);
    }
    public static void Defeat()
    {
        IsDefeat = true;
        PlayerBar.Update(PlayerBar.PlayerBarType.HEALTH, -Player.Hero.Health, 1f);        
        GameObject.Find("Game Manager").GetComponent<Game>().StartCoroutine(DefeatNumerator(5f));
    }
    public static void Pause(bool _status)
    {
        IsPaused = _status;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private static IEnumerator DefeatNumerator(float _time)
    {
        GameObject.Find("Canvas").GetComponent<Interface>().Close(GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel);
        CameraScript.SmoothMoveTo(Player.Hero, _time);
        CameraScript.Fade(0.5f, _time);
        CameraScript.Zoom(3, _time);
        yield return new WaitForSeconds(_time);
        Interface.Show(false);        
        GameObject.Find("Canvas").GetComponent<Interface>().Open(GameObject.Find("Canvas").GetComponent<Interface>().DefeatPanel);
        GameObject.Find("Game Manager").GetComponent<Game>().StopCoroutine(DefeatNumerator(_time));
    }
}
