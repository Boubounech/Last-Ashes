using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    public PlayerMovementRays playerMovementScript;
    public GameObject attackBox;
    public GameObject player;
    BoxCollider2D attackCollider;
    SpriteRenderer attackRenderer;

    private float offsetPlayerSizeX;
    private float offsetPlayerSizeY;
    private bool facingRight;
    private bool allowChangeFacing;
    private float lookUpPos;
    private float valueX;
    private float valueY;
    private float boxOffsetY;
    private bool isOnCooldown;
    [SerializeField] private float pogoPower;


    private void Start()
    {

        BoxCollider2D col = player.GetComponent<BoxCollider2D>();
        attackRenderer = attackBox.GetComponent<SpriteRenderer>();
        attackCollider = attackBox.GetComponent<BoxCollider2D>();
        isOnCooldown = false;
        allowChangeFacing = true;
        offsetPlayerSizeX = col.size.x * 1.5f;
        offsetPlayerSizeY = col.size.y;
        boxOffsetY = col.offset.y;
    }

    private void Update()
    {
        if (allowChangeFacing)
        {
            lookUpPos = playerMovementScript.getLook();
            facingRight = playerMovementScript.getFacing();
            if (lookUpPos > 0f)
            {
                valueX = 0f;
                valueY = offsetPlayerSizeY + boxOffsetY;
            }
            else if(lookUpPos < 0f)
            {
                if (playerMovementScript.isGrounded())
                {
                    lookUpPos = 0f; 
                }
                else
                {
                    valueX = 0f;
                    valueY = -offsetPlayerSizeY + boxOffsetY;
                }
            }
            if(lookUpPos == 0f)
            {
                valueY = 0.5f;
                if (facingRight)
                {
                    valueX = offsetPlayerSizeX;
                    attackRenderer.flipX = false;
                }
                else
                {
                    valueX = -offsetPlayerSizeX;
                    attackRenderer.flipX = true;
                }
            }

            attackBox.transform.localPosition = new Vector3(valueX, valueY, 0);
        }
    }


    private IEnumerator ToggleAttack()
    {
        isOnCooldown = true;
        allowChangeFacing = false;
        attackRenderer.enabled = true;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(0.2f);
        attackRenderer.enabled = false;
        attackCollider.enabled = false;


        yield return new WaitForSeconds(0.3f);
        allowChangeFacing = true;
        isOnCooldown = false;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isOnCooldown)
            {
                StartCoroutine(ToggleAttack());
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (lookUpPos < 0f)
            {
                Debug.Log("trigger Enemy Down");
                playerMovementScript.JumpAction(pogoPower);
            }
            Destroy(other.gameObject);
        }


    }
}
