using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
public class Sound : MonoBehaviour
{
    public enum PlayerSoundType { 
        HIT,
        DEATH,
        MISSSTRIKE,
        STRIKE,
        MAGIC,
        HEALING,
        WALKING,
        JUMP,
        LANDING
    }

    public enum GameEventSoundType
    {
        DEFEAT,
        PAUSE, 
        VICTORY,
        CLICK,
        LOCATION,
    }

    public AudioClip Pause;
    public AudioClip Location;
    public AudioClip MainMenu;
    public AudioClip ButtonClick;
    public AudioClip Run;
    public AudioClip MissStrike;
    public AudioClip Strike;
    public AudioClip Landing;
    public AudioClip Hit;
    public AudioClip EnemyHit;

    public void Play(string _sound)
    {
        switch (_sound)
        {
            case "Click":
                Play(GameEventSoundType.CLICK);
                break;
        }
    }

    public static void Play(PlayerSoundType _sound)
    {
        var source = new GameObject();
        source.transform.position = Player.Hero.transform.position;
        source.transform.SetParent(GameObject.Find("Sound Manager").transform);
        source.name = "Player Event Sound";
        switch (_sound)
        {
            case PlayerSoundType.MISSSTRIKE:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().MissStrike;
                break;
            case PlayerSoundType.STRIKE:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Strike;
                break;
            case PlayerSoundType.WALKING:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Run;
                break;
            case PlayerSoundType.HIT:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Hit;
                break;
            case PlayerSoundType.LANDING:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Landing;
                break;
        }
        source.AddComponent<AudioObject>().Play();
    }
    public static void Play(GameEventSoundType _sound)
    {
        var source = new GameObject();
        source.transform.position = Player.Hero.transform.position;
        source.transform.SetParent(GameObject.Find("Sound Manager").transform);
        source.name = "Game Event Sound";
        switch (_sound)
        {
            case GameEventSoundType.PAUSE:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Pause;
                break;
            case GameEventSoundType.CLICK:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().ButtonClick;
                break;
            case GameEventSoundType.LOCATION:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Location;
                break;            
        }
        source.AddComponent<AudioObject>().Play();
    }
}
