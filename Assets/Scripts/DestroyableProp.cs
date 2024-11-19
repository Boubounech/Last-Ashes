using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestroyableProp : MonoBehaviour
{
    public string attackLayer;
    public SpriteRenderer sprite;
    public ParticleSystem ps;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == attackLayer)
        {
            sprite.enabled = false;
            ps.Play();
            Invoke("DestroyThis", 0.5f);
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
