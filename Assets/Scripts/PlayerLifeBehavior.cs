using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeBehavior : MonoBehaviour
{

    [SerializeField] private float invicibilityTime;
    [SerializeField] private float damageKnockbackPower;
    public PlayerMovementRays playerMovementScript;

    private BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            LifePointsManager.instance.LoseHp();
            playerMovementScript.KnockbackFromDamage(damageKnockbackPower);
            StartCoroutine(InvicibilityTimerCoroutine());
        }
    }

    private IEnumerator InvicibilityTimerCoroutine()
    {
        col.enabled = false;
        yield return new WaitForSeconds(invicibilityTime);
        col.enabled = true;
    }
}
