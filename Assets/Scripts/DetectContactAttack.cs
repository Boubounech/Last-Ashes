using UnityEngine;

public class DetectContactAttack : MonoBehaviour
{
    public Combat combatScript;
    [SerializeField] private float attackDamage;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerEvents.OnPlayerHitDamageable.Invoke(attackDamage);
            combatScript.PogoOnDamage();
        }
    }
}
