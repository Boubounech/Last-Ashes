using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectContactAttack : MonoBehaviour
{
    public Combat combatScript;
    [SerializeField] private float attackDamage;
    [SerializeField] private bool enablePogo;
    private bool canPogo = true;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerEvents.OnPlayerHitDamageable.Invoke(attackDamage, other.gameObject);
            if (enablePogo && canPogo)
            {
                combatScript.PogoOnDamage();
                canPogo = false; 
                StartCoroutine(ResetPogoCooldown()); 
            }
        }
    }

    private IEnumerator ResetPogoCooldown()
    {
        yield return new WaitForSeconds(0.1f); 
        canPogo = true;
    }
}
