using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : AIBaseFlyingEnemy
{
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;

    private void Awake()
    {
        PlayerEvents.OnPlayerHitDamageable.AddListener(GetKnockback);
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
        float currentTime = 0;
        while (currentTime < knockbackTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, -player.position, knockbackSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(stunTime);
        this.canMove = true;
    }

}
