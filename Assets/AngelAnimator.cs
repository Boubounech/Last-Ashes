using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        AngelBoss.OnAttackCharging.AddListener(ShowAttackCharge);
    }

    private void ShowAttackCharge()
    {
        animator.SetTrigger("Attack");
    }

    public void SetAttackIsCharged()
    {
        AngelBoss.OnAttackReady.Invoke();
    }
}
