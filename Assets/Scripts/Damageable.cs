using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float lifePoints;

    private void Awake()
    {
        PlayerEvents.OnPlayerHitDamageable.AddListener(GetDamage);
    }

    private void GetDamage(float damage)
    {
        this.lifePoints -= damage;
        if(this.lifePoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
