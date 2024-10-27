using UnityEngine;

public class DetectContactAttack : MonoBehaviour
{
    public Combat combatScript;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerEvents.OnPlayerHitDamageable.Invoke();
            combatScript.DealAttackDamage(other.gameObject);
        }
    }
}
