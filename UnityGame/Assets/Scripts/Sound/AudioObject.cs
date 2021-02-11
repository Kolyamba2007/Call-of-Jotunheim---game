using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public void Play()
    {
        StartCoroutine(AudioCoroutine());
    }

    private IEnumerator AudioCoroutine()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy(gameObject);
    }
}
