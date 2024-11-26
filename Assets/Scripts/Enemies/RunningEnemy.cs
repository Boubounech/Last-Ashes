using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningEnemy : AIBaseEnemy
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpCooldown;
    private bool canJump;
    private Rigidbody2D rb;

    public override void Awake()
    {
        base.Awake();
        OnStartChasing.AddListener(delegate { StartRunning(); });
        OnStartPatrolling.AddListener(delegate { StopRunning(); });
        OnStartAttacking.AddListener(delegate { OnEndAttacking.Invoke(); });
        rb = GetComponent<Rigidbody2D>();
        canJump = true;
    }

    public void StartRunning()
    {
        SetSpeed(runSpeed);
    }

    public void StopRunning()
    {
        ResetSpeed();
    }


    public override void PlayerAbove()
    {
        if (canJump)
        {
            Debug.Log("jump");
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        canJump = false;
        float gravity = -Physics2D.gravity.y;
        float verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);

        Vector2 jumpVelocity = new Vector2(rb.velocity.x, verticalVelocity*1.25f);

        rb.velocity = jumpVelocity;

        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
}
