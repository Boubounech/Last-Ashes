
using System;
using UnityEngine;

public class DashingEnemy : PlayerDetector
{
    public enum AIState
    {
        Patrolling,
        Chasing,
        Attacking
    };

    [Serializable]
    public struct AIAttackZone
    {
        public Vector2 zone;
        public bool isDirectional;
        public string attackName;
    }

    [SerializeField] private float moveSpeed;

    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float timeBeforeDash = 0.75f;
    public float timeAfterDash = 0.5f;

    public AIState state;

    private void Start()
    {
        OnPlayerFound.AddListener(delegate { state = AIState.Chasing; });
        OnPlayerLost.AddListener(delegate { state = AIState.Patrolling; });
    }

    private void Update()
    {
        if (state == AIState.Patrolling)
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


    void FlipEnemy()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
