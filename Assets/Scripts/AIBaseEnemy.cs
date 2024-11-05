using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBaseEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public Vector2 detectionSize = new Vector2(5f, 2f); 
    public Vector2 headDetectionSize = new Vector2(1.5f, 0.5f);
    public bool showDetectionPlayerRectangle = true; 
    public bool showHeadDetectionRectangle = true; 

    private bool movingRight = false;
    private bool playerNearby = false;
    private bool playerAbove = false;

    public float wallRayDistance = 0.5f; 
    public float edgeRayDistance = 1.0f; 

    public Transform player; 

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        Vector2 detectionCenter = (Vector2)transform.position + Vector2.up * detectionSize.y / 2;
        playerNearby = Physics2D.OverlapBox(detectionCenter, detectionSize, 0f, playerLayer);

        Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 0.5f); 
        playerAbove = Physics2D.OverlapBox(headDetectionCenter, headDetectionSize, 0f, playerLayer);

        if (playerAbove)
        {
            return; 
        }

        if (playerNearby)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * direction, wallRayDistance, groundLayer);

        Vector2 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
        RaycastHit2D groundHit = Physics2D.Raycast(edgeRayOrigin, Vector2.down, edgeRayDistance, groundLayer);

        if (wallHit.collider != null || groundHit.collider == null)
        {
            movingRight = !movingRight;
            FlipEnemy();
        }
    }

    void ChasePlayer()
    {
        if (player.position.x > transform.position.x)
        {
            if (!movingRight) FlipEnemy();
            movingRight = true;
        }
        else
        {
            if (movingRight) FlipEnemy();
            movingRight = false;
        }

        transform.Translate(Vector2.right * (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime);
    }

    void FlipEnemy()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float direction = movingRight ? 1 : -1;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * wallRayDistance);

        Gizmos.color = Color.blue;
        Vector3 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
        Gizmos.DrawLine(edgeRayOrigin, edgeRayOrigin + Vector3.down * edgeRayDistance);

        if (showDetectionPlayerRectangle)
        {
            Gizmos.color = Color.green;
            Vector2 detectionCenter = (Vector2)transform.position + Vector2.up * detectionSize.y / 2;
            Gizmos.DrawWireCube(detectionCenter, detectionSize);
        }


        if (showHeadDetectionRectangle)
        {
            Gizmos.color = Color.yellow;
            Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 0.5f);
            Gizmos.DrawWireCube(headDetectionCenter, headDetectionSize);
        }
    }
}