using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBaseEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float baseMoveSpeed;
    [SerializeField] private bool actionPlayerAbove;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public Vector2 detectionSize = new Vector2(5f, 2f); 
    public Vector2 headDetectionSize = new Vector2(1.5f, 0.5f);
    public bool showDetectionPlayerRectangle = true; 
    public bool showHeadDetectionRectangle = true; 

    protected bool movingRight = false;
    private bool playerNearby = false;
    private bool playerAbove = false;
    protected bool canMove = true;

    public float wallRayDistance = 0.5f; 
    public float edgeRayDistance = 1.0f;
    public float attackRange = 5.0f;

    public Transform player;

    public enum State
    {
        None,
        Patrol,
        Chase,
        Attack
    }
    public State state;

    public UnityEvent OnStartPatrolling = new UnityEvent();
    public UnityEvent OnStartChasing = new UnityEvent();
    public UnityEvent OnStartAttacking = new UnityEvent();
    public UnityEvent OnEndAttacking = new UnityEvent();

    public virtual void Awake()
    {
        OnStartAttacking.AddListener(delegate { canMove = false; state = State.Attack; });
        OnEndAttacking.AddListener(delegate { canMove = true; state = State.None; });
    }

    void Start()
    {
        baseMoveSpeed = moveSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.None;
    }

    void Update()
    {
        if (state == State.Attack) return;
        if (canMove) {
            Vector2 detectionCenter = (Vector2)transform.position + Vector2.up * detectionSize.y / 2;
            playerNearby = Physics2D.OverlapBox(detectionCenter, detectionSize, 0f, playerLayer);

            Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 1f);
            playerAbove = Physics2D.OverlapBox(headDetectionCenter, headDetectionSize, 0f, playerLayer);
        
            if (playerAbove)
            {
                if (actionPlayerAbove)
                {
                    PlayerAbove();
                }
                else
                {
                    return;
                }

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
        if (state != State.Patrol)
        {
            state = State.Patrol;
            OnStartPatrolling.Invoke();
        }

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

    public virtual void ChasePlayer()
    {
        if (state != State.Chase)
        {
            state = State.Chase;
            OnStartChasing.Invoke();
        }

        float direction = movingRight ? 1 : -1;
        Vector2 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
        RaycastHit2D groundHit = Physics2D.Raycast(edgeRayOrigin, Vector2.down, edgeRayDistance, groundLayer);

        RaycastHit2D dashHit = Physics2D.Raycast(transform.position, Vector2.right * direction, attackRange, playerLayer);

        if (!groundHit.collider)
        {
            return; 
        }

        if (dashHit.collider != null)
        {
            state = State.Attack;
            OnStartAttacking.Invoke();
            //StartCoroutine(DashTowardsPlayer(direction));
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

    public virtual void PlayerAbove()
    {
        OnStartAttacking.Invoke();
    }

    void FlipEnemy()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void SetSpeed(float speed)
    {
        this.moveSpeed = speed;
    }

    public void ResetSpeed()
    {
        this.moveSpeed = baseMoveSpeed;
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
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * attackRange);
    }
}