using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float travelDistance;
    public LayerMask groundLayer;
    private Vector2 targetPosition;
    private bool isInitialized = false;

    public void Initialize(Vector2 playerPosition, Vector2 spawnPosition)
    {
        Vector2 direction = (playerPosition - spawnPosition).normalized;

        targetPosition = spawnPosition + direction * travelDistance;
        isInitialized = true;
    }

    void Update()
    {
        if (isInitialized)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
