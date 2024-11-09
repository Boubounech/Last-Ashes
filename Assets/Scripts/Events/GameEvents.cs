using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent OnPauseGame = new UnityEvent();
    public static UnityEvent OnResumeGame = new UnityEvent();
}
