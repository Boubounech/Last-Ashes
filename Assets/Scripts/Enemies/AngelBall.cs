using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelBall : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public float maxSpeed;
    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        rb.AddForce((player.position - transform.position) * Time.deltaTime * speed);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity / (rb.velocity.magnitude) * maxSpeed;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("TRIGGER WITH PLAYER");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("COLLIDE WITH PLAYER");
        }
    }
}
