using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    private Vector3 direction = new Vector3(1, 0, 0);
    public float aliveTime = 1f;
    public Combat combatScript;
    [SerializeField] private float fireballDamage;

    private void Start()
    {
        Invoke("EndLife", aliveTime);
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void EndLife()
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        if (direction.x < 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipY = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ok");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("hit");
            PlayerEvents.OnPlayerHitDamageable.Invoke(fireballDamage, other.gameObject);
        }
    }
}
