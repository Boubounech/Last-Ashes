using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : AIBaseEnemy
{
    [SerializeField] private float timeBeforeJump;
    [SerializeField] private float timeAfterJump;

    [SerializeField] private float jumpHeight;

    private Rigidbody2D rb;

    public override void Awake()
    {
        base.Awake();
        OnStartAttacking.AddListener(delegate { StartCoroutine(JumpTowardsPlayer(movingRight ? 1 : -1)); }); 
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator JumpTowardsPlayer(float direction)
    {
        Vector2 targetPosition = player.position;
        Vector2 startPosition = rb.position;
        float gravity = -Physics2D.gravity.y;
        float verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        float timeToApex = verticalVelocity / gravity;
        float totalJumpTime = timeToApex * 2;

        float horizontalDistance = targetPosition.x - startPosition.x;
        float horizontalVelocity = horizontalDistance / totalJumpTime;

        Vector2 jumpVelocity = new Vector2(horizontalVelocity, verticalVelocity);

        yield return new WaitForSeconds(timeBeforeJump);

        rb.velocity = jumpVelocity;

        yield return new WaitForSeconds(timeAfterJump);

        OnEndAttacking.Invoke();
    }


}
