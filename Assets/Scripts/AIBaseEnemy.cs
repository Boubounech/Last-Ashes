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
    private bool canMove = true;

    public float wallRayDistance = 0.5f; 
    public float edgeRayDistance = 1.0f;
    public float dashRayDistance = 5.0f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float timeBeforeDash = 0.75f;
    public float timeAfterDash = 0.5f;

    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector2 detectionCenter = (Vector2)transform.position + Vector2.up * detectionSize.y / 2;
        playerNearby = Physics2D.OverlapBox(detectionCenter, detectionSize, 0f, playerLayer);

        Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 1f);
        playerAbove = Physics2D.OverlapBox(headDetectionCenter, headDetectionSize, 0f, playerLayer);
        if (canMove)
        {
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
        float direction = movingRight ? 1 : -1;
        Vector2 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
        RaycastHit2D groundHit = Physics2D.Raycast(edgeRayOrigin, Vector2.down, edgeRayDistance, groundLayer);

        RaycastHit2D dashHit = Physics2D.Raycast(transform.position, Vector2.right * direction, dashRayDistance, playerLayer);

        if (!groundHit.collider)
        {
            return; 
        }

        if (dashHit.collider != null)
        {
            StartCoroutine(DashTowardsPlayer(direction));
        }
        else
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
    }

    IEnumerator DashTowardsPlayer(float direction)
    {
        canMove = false;
        yield return new WaitForSeconds(timeBeforeDash);

        float dashTime = 0;
        while (dashTime < dashDuration)
        {
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * direction, wallRayDistance, groundLayer);
            Vector2 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
            RaycastHit2D groundHit = Physics2D.Raycast(edgeRayOrigin, Vector2.down, edgeRayDistance, groundLayer);
            if(wallHit.collider != null || groundHit.collider == null)
            {
                break;
            }

            transform.Translate(Vector2.right * direction * dashSpeed * Time.deltaTime);
            dashTime += Time.deltaTime;
            yield return null; 
        }

        yield return new WaitForSeconds(timeAfterDash);
        canMove = true;
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
            Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 1f);
            Gizmos.DrawWireCube(headDetectionCenter, headDetectionSize);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * dashRayDistance);
    }
}