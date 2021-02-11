using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public void Cast(float _cooldown)
    {
        StartCoroutine(InterpolatedCooldown(_cooldown));
    }

    private IEnumerator InterpolatedCooldown(float _time)
    {
        float startAmount = GetComponent<Image>().fillAmount;
        float startTime = Time.time;
        while (GetComponent<Image>().fillAmount > 0)
        {
            float discoveredTime = Time.time - startTime;
            GetComponent<Image>().fillAmount = Mathf.Lerp(startAmount, 0, discoveredTime / _time);
            double cooldown = System.Math.Round(_time - discoveredTime, 1);
            yield return null;
        }
        GetComponent<Image>().fillAmount = 1;
        //GetComponent<Ability>().IsReady = true;
        StopCoroutine(InterpolatedCooldown(_time));
    }
}
