using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    private Transform playerCamObjective;
    public Transform bossCamObjective;

    public GameObject angelBoss;

    private void Awake()
    {
        angelBoss.GetComponent<Damageable>().OnDeath.AddListener(
            delegate { GameEvents.OnPlayerKilledBoss.Invoke(); });

        GameEvents.OnPlayerEnterBossZone.AddListener(SpawnBoss);
        GameEvents.OnPlayerKilledBoss.AddListener(FreePlayer);
        GameEvents.OnPlayerKilledBoss.AddListener(delegate
        {
            if (angelBoss.name == "AngelBoss")
                GameManager.GiveLeftCoin();
            else
                GameManager.GiveRightCoin();
        });
    }

    private void Start()
    {
        playerCamObjective = cam.Follow;
        if (angelBoss.gameObject.activeInHierarchy)
        {
            Debug.LogError("Angel should be disabled by default in this scene");
        }
    }

    private void SpawnBoss()
    {
        Debug.Log("Load boss!");
        cam.Follow = bossCamObjective;
        angelBoss.gameObject.SetActive(true);
    }

    private void FreePlayer()
    {
        cam.Follow = playerCamObjective;
    }
}
