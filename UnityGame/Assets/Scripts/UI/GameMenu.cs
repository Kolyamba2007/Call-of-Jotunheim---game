using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void Open()
    {
        GetComponent<Animator>().ResetTrigger("Close");
        GetComponent<Animator>().SetTrigger("Open");

        Sound.Play(Sound.GameEventSoundType.PAUSE);
    }
    public void Close()
    {
        GetComponent<Animator>().ResetTrigger("Open");
        GetComponent<Animator>().SetTrigger("Close");
        gameObject.transform.SetAsFirstSibling();
    }
    public void Idle()
    {
        GetComponent<Animator>().ResetTrigger("Open");
        GetComponent<Animator>().ResetTrigger("Close");
        GetComponent<Animator>().SetTrigger("Idle");
        gameObject.SetActive(false);
    }
}
