using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : AIBaseFlyingEnemy
{
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;


    private Rigidbody2D rb;

    private void Awake()
    {
        PlayerEvents.OnPlayerHitDamageable.AddListener(GetKnockback);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void GetKnockback(float damage, GameObject reciever)
    {
        if(this.gameObject == reciever)
        {
            StartCoroutine(KnockbackAction());

        }
    }

    private IEnumerator KnockbackAction()
    {
        this.canMove = false;

        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.velocity = knockbackDirection * knockbackSpeed;

        yield return new WaitForSeconds(knockbackTime);

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(stunTime);

        this.canMove = true;
    }


}
