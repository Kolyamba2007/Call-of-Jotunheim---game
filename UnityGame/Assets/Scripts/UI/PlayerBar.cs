using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public static bool PlayerHealthFrozen = false;
    public static bool PlayerManaFrozen = false;

    private void Update()
    {
        //if (!PlayerHealthFrozen && !Player.Hero.IsDead) GameObject.Find("Player Bar").transform.GetChild(1).GetComponentInChildren<Image>().fillAmount = Player.Hero.Health / Player.Hero.MaxHealth;
    }

    // ИЗМЕНЕНИЕ ПОЛОСКИ ИГРОКА
    public enum PlayerBarType { HEALTH, MANA }
    public static void Update(PlayerBarType _type, float _value, float _time)
    {
        if (_type == PlayerBarType.HEALTH) PlayerHealthFrozen = true;
        else PlayerManaFrozen = true;

        if (Player.Hero != null && !Player.Hero.IsDead)
        {
            if (Player.Hero.Health + _value >= 0) GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedBar(_type, _value, _time));
            else GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedBar(_type, -Player.Hero.Health, _time));
        }
    }
    public static IEnumerator InterpolatedBar(PlayerBarType _type, float _value, float _time)
    {
        float startTime = Time.time;
        switch (_type)
        {
            case PlayerBarType.HEALTH:
                while (GameObject.Find("Player Bar").transform.GetChild(0).GetComponent<Image>().fillAmount != (Player.Hero.Health + _value) / Player.Hero.MaxHealth)
                {
                    float elapsedTime = Time.time - startTime;
                    float delta = Mathf.Lerp(Player.Hero.Health, (Player.Hero.Health + _value), elapsedTime / _time);
                    GameObject.Find("Player Bar").transform.GetChild(0).GetComponent<Image>().fillAmount = delta / Player.Hero.MaxHealth;
                    yield return null;
                }
                PlayerHealthFrozen = false;
                break;
            case PlayerBarType.MANA:
                while (GameObject.Find("Player Bar").transform.GetChild(1).GetComponent<Image>().fillAmount != (Player.Hero.Health + _value) / Player.Hero.MaxMana)
                {
                    float elapsedTime = Time.time - startTime;
                    float delta = Mathf.Lerp(Player.Hero.Health, (Player.Hero.Health + _value), elapsedTime / _time);
                    GameObject.Find("Player Bar").transform.GetChild(1).GetComponent<Image>().fillAmount = delta / Player.Hero.MaxMana;
                    yield return null;
                }
                PlayerManaFrozen = false;
                break;
        }
        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedBar(_type, _value, _time));
    }
}
