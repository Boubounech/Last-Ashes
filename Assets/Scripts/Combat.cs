using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    public PlayerMovementRays playerMovementScript;

    [Header("Sword")]
    public GameObject attackBox;
    public GameObject player;
    public float timeAttack = 0.2f;
    public float cooldownAttack = 0.3f;
    private bool isAttackOnCooldown;

    [Header("Fireball")]
    public Transform fireballSpawn;
    public GameObject fireballPrefab;
    public float cooldownFireball = 0.4f;
    private bool isFireballOnCooldown;
    public float fireballVitalEnergyCost = 10;

    Collider2D attackCollider;
    SpriteRenderer attackRenderer;

    private float offsetPlayerSizeX;
    private float offsetPlayerSizeY;
    private bool facingRight;
    private bool allowChangeFacing;
    private float lookUpPos;
    private float valueX;
    private float valueY;
    private float boxOffsetY;
    private float height;
    [SerializeField] private float pogoPower;


    private void Start()
    {

        BoxCollider2D col = player.GetComponent<BoxCollider2D>();
        attackRenderer = attackBox.GetComponent<SpriteRenderer>();
        attackCollider = attackBox.GetComponent<Collider2D>();
        isAttackOnCooldown = false;
        allowChangeFacing = true;
        offsetPlayerSizeX = col.size.x * 1.5f;
        offsetPlayerSizeY = col.size.y;
        boxOffsetY = col.offset.y;
        height = attackBox.transform.localPosition.y;
    }

    private void Update()
    {
        if (allowChangeFacing)
        {
            lookUpPos = playerMovementScript.getLook();
            facingRight = playerMovementScript.getFacing();
            Quaternion rot = Quaternion.identity;
            if (lookUpPos > 0f)
            {
                attackRenderer.flipX = false;
                if (!facingRight)
                    attackRenderer.flipY = true;
                valueX = 0f;
                valueY = offsetPlayerSizeY + boxOffsetY;
                rot = Quaternion.Euler(0, 0, 90);
            }
            else if(lookUpPos < 0f)
            {
                if (playerMovementScript.isGrounded())
                {
                    lookUpPos = 0f; 
                }
                else
                {
                    attackRenderer.flipX = false;
                    if (!facingRight)
                        attackRenderer.flipY = true;
                    valueX = 0f;
                    valueY = -offsetPlayerSizeY + boxOffsetY;
                    rot = Quaternion.Euler(0, 0, -90);
                }
            }
            if(lookUpPos == 0f)
            {
                attackRenderer.flipY = false;
                valueY = height;
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
            attackBox.transform.localRotation = rot;
        }
    }


    private IEnumerator ToggleAttack()
    {
        isAttackOnCooldown = true;
        allowChangeFacing = false;
        attackRenderer.enabled = true;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(timeAttack);
        attackRenderer.enabled = false;
        attackCollider.enabled = false;

        yield return new WaitForSeconds(cooldownAttack);
        allowChangeFacing = true;
        isAttackOnCooldown = false;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isAttackOnCooldown)
            {
                StartCoroutine(ToggleAttack());
            }
        }
    }

    private IEnumerator ToggleFireball()
    {
        isFireballOnCooldown = true;
        allowChangeFacing = false;
        Vector3 position = new Vector3(
            facingRight ? fireballSpawn.position.x : fireballSpawn.position.x - 2*fireballSpawn.localPosition.x,
            fireballSpawn.position.y,
            fireballSpawn.position.z);
        Fireball fb = Instantiate(fireballPrefab, position, Quaternion.identity).GetComponent<Fireball>();
        fb.SetDirection(facingRight ? Vector3.right : -Vector3.right);
        fb.combatScript = this;

        VitalEnergyManager.instance.RemoveTime(fireballVitalEnergyCost);

        yield return new WaitForSeconds(cooldownFireball);
        allowChangeFacing = true;
        isFireballOnCooldown = false;
    }

    public void Fireball(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isFireballOnCooldown)
            {
                StartCoroutine(ToggleFireball());
            }

        }
    }

    public void DealAttackDamage(GameObject reciever)
    {
        if (lookUpPos < 0f)
        {
            playerMovementScript.JumpAction(pogoPower);
        }
        Destroy(reciever);
    }

    
}
