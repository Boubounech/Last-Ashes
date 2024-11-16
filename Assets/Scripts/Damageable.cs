using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float lifePoints;

    public UnityEvent<float> OnDamaged = new UnityEvent<float>();
    public UnityEvent OnDeath = new UnityEvent();

    private void Awake()
    {
        PlayerEvents.OnPlayerHitDamageable.AddListener(GetDamage);
    }

    private void GetDamage(float damage, GameObject reciever)
    {
        if(reciever == this.gameObject)
        {
            this.lifePoints -= damage;
            OnDamaged.Invoke(damage);
            if (this.lifePoints <= 0)
            {
                OnDeath.Invoke();
                Destroy(this.gameObject);
            }
        }
        
    }

}
