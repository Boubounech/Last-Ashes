using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovementRays : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float timeRememberInputs = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private bool hasControl = true;
    [SerializeField] private float timerKnockbackDamage;

    [Header("Move")]
    [SerializeField] private float speed = 4f;
    private float lookUpPosition;
    private float horizontalMovement;
    private float wantedHorizontalMovement;
    private bool facingRight = true;

    [Header("Jump")]
    [SerializeField] private float jumpingPower = 200f;
    [SerializeField] private float jumpHoldPower = 60f;
    [SerializeField] private bool wantsToJump = false;
    private float jumpHoldTimer = 0;
    [SerializeField] private float maxJumpHoldTime = 0.1f;
    [SerializeField] private float shortestDistToGround = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool hasAlreadyJump = false;
    [SerializeField] private float airZone = 0.5f;

    [Header("Double Jump")]
    [SerializeField] private bool allowDoubleJump = true;
    [SerializeField] private float doubleJumpPower = 100f;
    [SerializeField] private float shortestDistToGroundForDJ = 0.3f;
    private bool canDoubleJump = false;
    private bool hasJumpedOnce = false;
    private bool hasCanceledOnce = false;

    [Header("Dash")]
    [SerializeField] private bool allowDash = true;
    [SerializeField] private float dashPower = 10;
    [SerializeField] private float dashLength = 0.2f;
    [SerializeField] private float dashReloadTime = 0.5f;
    private bool wantsToDash = false;
    private bool isDashing = false;
    private bool canDash = true;

    [Header("Gravity")]
    [SerializeField] private float airGravityScale = 0.75f;
    [SerializeField] private float fallGravityScale = 2;
    private float normalGravityScale;
    [SerializeField] private float maxVerticalSpeed;

    [Header("WallJump")]
    [SerializeField] private bool allowWallJump;
    [SerializeField] private float timerChangeOrientation;
    [SerializeField] private float slideSpeed;
    private bool allowChangeOrientation = true;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private Vector2 rightOffset;
    private Vector2 farRightOffset;

    private Vector2 upOffset;
    private Vector2 downOffset;

    public UnityAction OnPlayerRegainControl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        normalGravityScale = rb.gravityScale;
        col = GetComponent<BoxCollider2D>();
  

        rightOffset = new Vector2(col.size.x / 3f, 0);
        farRightOffset = rightOffset * 3.5f;

        upOffset = col.offset + new Vector2(0, col.size.y / 3f);
        downOffset = col.offset + new Vector2(0, - col.size.y / 3f);
    }

    private void FixedUpdate()
    {
        float xVelocity = 0f;
        float yVelocity = 0f;

        if (hasControl)
        {
            if (allowDash && wantsToDash && !isDashing && canDash)
            {
                isDashing = true;
                rb.gravityScale = 0;
                setWantsToDashTo(false);
                StartCoroutine(dashCoroutine());
            }

            if (!allowDash || !isDashing)
            {
                horizontalMovement = wantedHorizontalMovement;

                if (wantsToJump && jumpHoldTimer < maxJumpHoldTime && rb.velocity.y > 0 && hasJumpedOnce != hasCanceledOnce)
                {
                    rb.AddForce(Vector2.up * jumpHoldPower);
                    jumpHoldTimer += Time.fixedDeltaTime;
                }
                else if (wantsToJump && (isGrounded() || (isAgainstWall() && allowWallJump)))
                {
                    if (!hasAlreadyJump)
                    {
                        JumpAction(jumpingPower);
                        hasJumpedOnce = true;
                        hasAlreadyJump = true;
                        jumpHoldTimer = 0;
                    }
                }
                else if (allowDoubleJump && wantsToJump && canDoubleJump && hasJumpedOnce && hasCanceledOnce && !isTooNearToDJ())
                {
                    jumpHoldTimer += maxJumpHoldTime;
                    canDoubleJump = false;
                    hasJumpedOnce = true;
                    hasAlreadyJump = true;
                    JumpAction(doubleJumpPower);
                }

                if (isAgainstWall() && allowWallJump)
                {
                    yVelocity = -slideSpeed;
                }
                else
                {
                    if (Math.Abs(rb.velocity.y) < airZone)
                    {
                        rb.gravityScale = airGravityScale;
                    }
                    else if (rb.velocity.y < 0)
                    {
                        rb.gravityScale = fallGravityScale;
                    }
                    else
                    {
                        rb.gravityScale = normalGravityScale;
                    }
                    yVelocity = rb.velocity.y >= 0 ? Math.Min(rb.velocity.y, maxVerticalSpeed) : Math.Max(rb.velocity.y, -maxVerticalSpeed);
                }

            }
        }

        if (allowChangeOrientation)
        {
            xVelocity = horizontalMovement * speed;
        }
        else
        {
            xVelocity = rb.velocity.x;
        }

        if (Math.Abs(xVelocity) > 0.03)
        {
            animator.SetBool("IsMoving", true);
        } else
        {
            animator.SetBool("IsMoving", false);
        }
        if (xVelocity < 0)
        {
            facingRight = false;
            animator.transform.localRotation = Quaternion.Euler(0, 180, 0);

        } else if (xVelocity > 0)
        {
            facingRight = true;
            animator.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (hasControl)
        {
            rb.velocity = new Vector2(xVelocity, yVelocity);
        }
    }



    public void JumpAction(float jumpPower)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);

        if (isAgainstWall() && allowWallJump)
        {
            BounceOppositeDirection(jumpPower, timerChangeOrientation);
            hasCanceledOnce = false;
            canDoubleJump = true;
        }

        rb.AddForce(Vector2.up * jumpPower);
    }

    public void KnockbackFromDamage(float knockbackPower)
    {
        rb.gravityScale = normalGravityScale;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * 1.5f * knockbackPower);
        BounceOppositeDirection(knockbackPower, timerKnockbackDamage);
    }

    public void BounceOppositeDirection(float jumpPower, float timerCooldown)
    {
        Vector2 oppositeDirection = facingRight ? Vector2.left : Vector2.right;
        rb.AddForce(oppositeDirection * jumpPower);
        StopCoroutine(cooldownChangeOrientation(timerCooldown));
        StartCoroutine(cooldownChangeOrientation(timerCooldown));

    }

    public bool isGrounded()
    {
        //Debug.DrawRay(rb.position, Vector2.down, Color.green, shortestDistToGround);
        //Debug.DrawRay(rb.position + rightOffset, Vector2.down, Color.green, shortestDistToGround);
        //Debug.DrawRay(rb.position - rightOffset, Vector2.down, Color.green, shortestDistToGround);
        //Debug.DrawRay(rb.position + farRightOffset, Vector2.down, Color.green, shortestDistToGround);
        //Debug.DrawRay(rb.position - farRightOffset, Vector2.down, Color.green, shortestDistToGround);
        bool i =
            Physics2D.Raycast(rb.position, Vector2.down, shortestDistToGround, groundLayer).collider != null
            || Physics2D.Raycast(rb.position + rightOffset, Vector2.down, shortestDistToGround, groundLayer).collider != null
            || Physics2D.Raycast(rb.position - rightOffset, Vector2.down, shortestDistToGround, groundLayer).collider != null
            || (!hasJumpedOnce && Physics2D.Raycast(rb.position + farRightOffset, Vector2.down, shortestDistToGround, groundLayer).collider != null)
            || (!hasJumpedOnce && Physics2D.Raycast(rb.position - farRightOffset, Vector2.down, shortestDistToGround, groundLayer).collider != null);
        if (i)
        {
            canDoubleJump = true;
            hasJumpedOnce = false;
            hasCanceledOnce = false;
        }
        return i;
    }

    private bool isTooNearToDJ()
    {
        Vector2 offset = new Vector2(col.size.x / 2f, 0);
        return Physics2D.Raycast(rb.position, Vector2.down, shortestDistToGroundForDJ, groundLayer).collider != null
            || Physics2D.Raycast(rb.position + offset, Vector2.down, shortestDistToGroundForDJ, groundLayer).collider != null
            || Physics2D.Raycast(rb.position - offset, Vector2.down, shortestDistToGroundForDJ, groundLayer).collider != null;
    }

    private bool isAgainstWall()
    {
        float wallCheckDistance = 0.35f;  
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;

        RaycastHit2D upperRay = Physics2D.Raycast(rb.position + upOffset, rayDirection, wallCheckDistance, groundLayer);
        RaycastHit2D lowerRay = Physics2D.Raycast(rb.position + downOffset, rayDirection, wallCheckDistance, groundLayer);

        Debug.DrawRay(rb.position + upOffset, rayDirection * wallCheckDistance, Color.red);
        Debug.DrawRay(rb.position + downOffset, rayDirection * wallCheckDistance, Color.red);

        bool isTouchingWall = upperRay.collider != null || lowerRay.collider != null;
        /*if (isTouchingWall)
        {
            canDoubleJump = true;
            hasJumpedOnce = false;
            hasCanceledOnce = false;
        }*/
        return isTouchingWall;
    }

    IEnumerator dashCoroutine()
    {
        isDashing = true;
        canDash = false;
        horizontalMovement = dashPower * (facingRight ? 1 : -1);
        yield return new WaitForSeconds(dashLength);
        isDashing = false;
        yield return new WaitForSeconds(dashReloadTime);
        canDash = true;
    }

    IEnumerator cooldownChangeOrientation(float time)
    {
        allowChangeOrientation = false;
        hasControl = false;
        yield return new WaitForSeconds(time);
        allowChangeOrientation = true;
        hasControl = true;
    }

    private void setWantsToJumpTo(bool wantsTo)
    {
        wantsToJump = wantsTo;
        if (wantsTo)
        {
            StopCoroutine(resetJumpInputCoroutine());
            StartCoroutine(resetJumpInputCoroutine());
        }
    }

    private IEnumerator resetJumpInputCoroutine()
    {
        yield return new WaitForSeconds(timeRememberInputs);
        if (jumpHoldTimer < maxJumpHoldTime)
        {
            yield return new WaitForSeconds(maxJumpHoldTime - jumpHoldTimer + Time.fixedDeltaTime);
        }
        wantsToJump = false;
    }

    private void setWantsToDashTo(bool wantsTo)
    {
        wantsToDash = wantsTo;
        if (wantsTo)
        {
            StopCoroutine(resetDashInputCoroutine());
            StartCoroutine(resetDashInputCoroutine());
        }
    }

    private IEnumerator resetDashInputCoroutine()
    {
        yield return new WaitForSeconds(timeRememberInputs);
        wantsToDash = false;
    }


    public void move(InputAction.CallbackContext context)
    {
        wantedHorizontalMovement = context.ReadValue<Vector2>().x;
        lookUpPosition = context.ReadValue<Vector2>().y;
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            setWantsToJumpTo(true);
        }
        else if (context.canceled)
        {
            hasCanceledOnce = true;
            setWantsToJumpTo(false);
            hasAlreadyJump = false;
        }
    }

    public void dash(InputAction.CallbackContext context)
    {
        if (context.performed && allowDash)
        {
            setWantsToDashTo(true);
        }
    }

    public bool getFacing()
    {
        return this.facingRight;
    }

    public float getLook()
    {
        return this.lookUpPosition;
    }

    public void TakeControlAndMoveTo(Vector3 newPosition)
    {
        hasControl = false;
        rb.velocity = new Vector2(Mathf.Sign(newPosition.x - transform.position.x) * speed, rb.velocity.y);
        float timeToMove = Vector3.Distance(transform.position, newPosition) / speed;
        Invoke("AllowMovement", timeToMove);
    }

    private void AllowMovement()
    {
        hasControl = true;
        OnPlayerRegainControl.Invoke();
    }
}
