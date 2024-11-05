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

    private void GetDamage(float damage, GameObject reciever)
    {
        if(reciever == this.gameObject)
        {
            this.lifePoints -= damage;
            if (this.lifePoints <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

}
