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
        GameEvents.OnPlayerEnterBossZone.AddListener(SpawnBoss);
        GameEvents.OnPlayerKilledBoss.AddListener(FreePlayer);
        angelBoss.GetComponent<Damageable>().OnDeath.AddListener(
            delegate { GameEvents.OnPlayerKilledBoss.Invoke(); });
    }

    private void Start()
    {
        playerCamObjective = cam.Follow;
    }

    private void SpawnBoss()
    {
        Debug.Log("Load boss");
        cam.Follow = bossCamObjective;
        angelBoss.gameObject.SetActive(true);
    }

    private void FreePlayer()
    {
        cam.Follow = playerCamObjective;
    }
}
