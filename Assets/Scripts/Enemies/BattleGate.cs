using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGate : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        GameEvents.OnPlayerEnterBossZone.AddListener(ShowGate);
        GameEvents.OnPlayerKilledBoss.AddListener(HideGate);
    }

    private void ShowGate()
    {
        animator.SetTrigger("Show");
    }

    private void HideGate()
    {
        animator.SetTrigger("Hide");
    }
}
