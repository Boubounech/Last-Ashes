using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBaseFlyingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public Vector2 detectionSize;

    public bool showDetectionPlayerRectangle = true;

    private bool playerNearby = false;
    protected bool canMove = true;

    public Transform player;

    public enum State
    {
        None,
        Patrol,
        Chase
    }

    public State state;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.None;
    }

    public virtual void Update()
    {
        if (canMove)
        {
            Vector2 detectionCenter = (Vector2)transform.position;
            playerNearby = Physics2D.OverlapBox(detectionCenter, detectionSize, 0f, playerLayer);
            if (playerNearby)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
    }

    public virtual void ChasePlayer()
    {
        if (state != State.Chase)
        {
            state = State.Chase;
        }
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    public virtual void Patrol()
    {
        if (state != State.Patrol)
        {
            state = State.Patrol;
        }
    }

    void OnDrawGizmos()
    {
        if (showDetectionPlayerRectangle)
        {
            Gizmos.color = Color.green;
            Vector2 detectionCenter = (Vector2)transform.position;
            Gizmos.DrawWireCube(detectionCenter, detectionSize);
        }

    }
}
