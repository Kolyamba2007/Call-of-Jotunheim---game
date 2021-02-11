using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    public static bool BossHealthFrozen = false;

    public static void Show(bool _status, float _time)
    {
        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedAppearance(_status, _time));
    }

    // ИЗМЕНЕНИЕ ПОЛОСКИ БОССА
    public static void Update(float _from, float _to, float _time)
    {
        BossHealthFrozen = true;

        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedBar(_from, _to, _time));
    }
    public static IEnumerator InterpolatedBar(float _from, float _to, float _time)
    {
        float startTime = Time.time;

        while (GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(0).GetComponent<Image>().fillAmount != _to / Player.Hero.MaxHealth)
        {
            float elapsedTime = Time.time - startTime;
            float delta = Mathf.Lerp(_from, _to, elapsedTime / _time);
            GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(0).GetComponent<Image>().fillAmount = delta / Player.Hero.MaxHealth;
            yield return null;
        }
        BossHealthFrozen = false;

        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedBar(_from, _to, _time));
    }
    public static IEnumerator InterpolatedAppearance(bool _status, float _time)
    {
        float startTime = Time.time;
        Color _healthColor = GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(0).GetComponent<Image>().color;
        Color _nameColor = GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(1).GetComponent<Text>().color;

        if (_status)
        {
            while (_healthColor != new Color(_healthColor.r, _healthColor.g, _healthColor.b, 1))
            {
                float elapsedTime = Time.time - startTime;
                Color deltaHealthColor = Color.Lerp(_healthColor, new Color(_healthColor.r, _healthColor.g, _healthColor.b, 1), elapsedTime / _time);
                Color deltaNameColor = Color.Lerp(_nameColor, new Color(_nameColor.r, _nameColor.g, _nameColor.b, 1), elapsedTime / _time);
                GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(0).GetComponent<Image>().color = deltaHealthColor;
                GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(1).GetComponent<Text>().color = deltaNameColor;
                yield return null;
            }
        }
        else
        {
            while (_healthColor != new Color(_healthColor.r, _healthColor.g, _healthColor.b, 0))
            {
                float elapsedTime = Time.time - startTime;
                Color deltaHealthColor = Color.Lerp(_healthColor, new Color(_healthColor.r, _healthColor.g, _healthColor.b, 0), elapsedTime / _time);
                Color deltaNameColor = Color.Lerp(_nameColor, new Color(_nameColor.r, _nameColor.g, _nameColor.b, 0), elapsedTime / _time);
                GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(0).GetComponent<Image>().color = deltaHealthColor;
                GameObject.Find("Canvas").GetComponent<Interface>().BossBar.transform.GetChild(1).GetComponent<Text>().color = deltaNameColor;
                yield return null;
            }
        }

        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedAppearance(_status, _time));
    }
}
