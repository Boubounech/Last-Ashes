using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimatorManager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        PlayerEvents.OnPlayerAttack.AddListener(Attack);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack(PlayerEvents.Attack attack)
    {
        transform.localScale = new Vector3(1, 1, 1);
        if (attack.direction.y > 0f)
        {
            animator.SetTrigger("AttackUp");
        } else if (attack.direction.y < 0f)
        {
            animator.SetTrigger("AttackDown");
        } else
        {
            if (attack.direction.x < 0f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            animator.SetTrigger("Attack");
        }
    }

    public void EmitEndOfAttack()
    {
        PlayerEvents.OnPlayerFinishAttack.Invoke();
    }

    public void EmitPlayerCanAttack()
    {
        PlayerEvents.OnPlayerCanAttack.Invoke();
    }
}
