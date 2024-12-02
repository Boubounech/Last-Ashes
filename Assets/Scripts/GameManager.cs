using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool hasLeftCoinPart = false;
    public static bool hasRightCoinPart = false;


    private void Awake()
    {
        GameEvents.OnPauseGame.AddListener(PauseGame);
        GameEvents.OnResumeGame.AddListener(ResumeGame);
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void SetCoin(bool left, bool right)
    {
        hasLeftCoinPart = left;
        hasRightCoinPart = right;
    }

    public static void GiveLeftCoin()
    {
        hasLeftCoinPart = true;
        GameEvents.OnPlayerObtainLeftCoin.Invoke();
    }

    public static void GiveRightCoin()
    {
        hasRightCoinPart = true;
        GameEvents.OnPlayerObtainRightCoin.Invoke();
    }
}
