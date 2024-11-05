using UnityEngine;

public class DetectContactAttack : MonoBehaviour
{
    public Combat combatScript;
    [SerializeField] private float attackDamage;
    [SerializeField] private bool enablePogo;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerEvents.OnPlayerHitDamageable.Invoke(attackDamage, other.gameObject);
            if (enablePogo)
            {
                combatScript.PogoOnDamage();
            }

        }
    }
}
