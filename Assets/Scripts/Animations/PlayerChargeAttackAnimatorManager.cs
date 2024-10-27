using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttackAnimatorManager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        PlayerEvents.OnPlayerChargeAttack.AddListener(delegate { animator.SetBool("Charging", true); });
        PlayerEvents.OnPlayerInterruptChargeAttack.AddListener(delegate { animator.SetBool("Charging", false); });
        PlayerEvents.OnPlayerReleaseChargeAttack.AddListener(delegate { animator.SetTrigger("Release"); });
        PlayerEvents.OnPlayerFinishChargeAttack.AddListener(delegate { animator.SetBool("Charging", false); });

        PlayerEvents.OnPlayerHorizontalFacing.AddListener(FaceForward);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FaceForward(float dir)
    {
        if (dir < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void EmitEndOfChargeAttack()
    {
        PlayerEvents.OnPlayerFinishChargeAttack.Invoke();
    }

    public void EmitChargeChargeAttack()
    {
        PlayerEvents.OnPlayerChargeChargeAttack.Invoke();
    }
}
