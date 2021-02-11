using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<object[]> PlayerDied;
    public static Action<object[]> PlayerCreated;
    public static Action<object[]> PlayerRevived;
    public static Action<object[]> PlayerAttacked;
    public static Action<object[]> PlayerAttacking;

    public static Action<Unit> UnitDied;

    public static Action<object[]> GameStarted;
    public static Action<object[]> GameEnded;
    public static Action<object[]> GameDefeat;
    public static Action<object[]> GamePaused;

    public static Action<GameObject> SoundPlayed;
    public static Action<GameObject> SoundPlaying;
}
