using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementRays : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float timeRememberInputs = 0.1f;
    [SerializeField] private Animator animator;

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

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private Vector2 rightOffset;
    private Vector2 farRightOffset;

    private Vector2 upOffset;
    private Vector2 downOffset;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravityScale = rb.gravityScale;
        col = GetComponent<BoxCollider2D>();

  

        rightOffset = new Vector2(col.size.x / 3f, 0);
        farRightOffset = rightOffset * 3.5f;

        upOffset = col.offset + new Vector2(0, col.size.y / 3f);
        downOffset = col.offset + new Vector2(0, - col.size.y / 3f);
    }

    private void FixedUpdate()
    {
        float xVelocity = 0;
        float yVelocity = 0;
        

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
            else if (wantsToJump && isGrounded())
            {
                if (!hasAlreadyJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    hasJumpedOnce = true;
                    hasAlreadyJump = true;
                    jumpHoldTimer = 0;
                    JumpAction(jumpingPower);
                }
            }
            else if (allowDoubleJump && wantsToJump && canDoubleJump && hasJumpedOnce == hasCanceledOnce && !isTooNearToDJ())
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                jumpHoldTimer += maxJumpHoldTime;
                canDoubleJump = false;
                hasJumpedOnce = true;
                hasAlreadyJump = true;
                JumpAction(doubleJumpPower);
            }

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

        xVelocity = horizontalMovement * speed;
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
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    public void JumpAction(float jumpPower)
    {
        rb.AddForce(Vector2.up * jumpPower);
    }

    public void ResetVelocity()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
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
        Debug.DrawRay(rb.position + upOffset + rightOffset, Vector2.right, Color.red, shortestDistToGround);
        Debug.DrawRay(rb.position + downOffset + rightOffset, Vector2.right, Color.red, shortestDistToGround);
        Debug.DrawRay(rb.position + upOffset - rightOffset, Vector2.left, Color.red, shortestDistToGround);
        Debug.DrawRay(rb.position + downOffset - rightOffset, Vector2.left, Color.red, shortestDistToGround);
        bool i =
            Physics2D.Raycast(rb.position + upOffset + rightOffset, Vector2.right, shortestDistToGround, groundLayer).collider != null
            || Physics2D.Raycast(rb.position + downOffset + rightOffset, Vector2.right, shortestDistToGround, groundLayer).collider != null
            || Physics2D.Raycast(rb.position + upOffset - rightOffset, Vector2.left, shortestDistToGround, groundLayer).collider != null
            || Physics2D.Raycast(rb.position + downOffset - rightOffset, Vector2.left, shortestDistToGround, groundLayer).collider != null;
        if (i)
        {
            canDoubleJump = true;
            hasJumpedOnce = false;
            hasCanceledOnce = false;
        }
        return i;
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
}
