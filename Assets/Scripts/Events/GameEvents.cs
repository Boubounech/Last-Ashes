using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent OnPauseGame = new UnityEvent();
    public static UnityEvent OnResumeGame = new UnityEvent();

    public static UnityEvent OnMapOpened = new UnityEvent();
    public static UnityEvent OnMapClosed = new UnityEvent();

    public static UnityEvent OnPlayerEnterBossZone = new UnityEvent();
    public static UnityEvent OnPlayerKilledBoss = new UnityEvent();

    public static UnityEvent OnPlayerObtainLeftCoin = new UnityEvent();
    public static UnityEvent OnPlayerObtainRightCoin = new UnityEvent();
}
