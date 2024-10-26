using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    public PlayerMovementRays playerMovementScript;
    private bool facingRight;

    [Header("Attack")]
    public float pogoPower;
    private bool isAttackOnCooldown;

    [Header("Fireball")]
    public Transform fireballSpawn;
    public GameObject fireballPrefab;
    public float cooldownFireball = 0.4f;
    private bool isFireballOnCooldown;
    public float fireballVitalEnergyCost = 10;

    [Header("ChargedAttack")]
    public Animator chargedAnimator;
    public float chargeTime;
    public float impactTime;
    private Coroutine chargedCoroutine;
    private bool hasReducedSpeed = false;

    private void Awake()
    {
        PlayerEvents.OnPlayerAttack.AddListener(UpdateOnAttack);
        PlayerEvents.OnPlayerCanAttack.AddListener(UpdateOnCanAttack);
    }

    private void Start()
    {
        facingRight = playerMovementScript.getFacing();
    }

    private void Update()
    {
    }

    private void UpdateOnAttack(PlayerEvents.Attack attack)
    {
        isAttackOnCooldown = true;
    }

    private void UpdateOnCanAttack()
    {
        isAttackOnCooldown = false;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isAttackOnCooldown)
            {
                PlayerEvents.Attack attack = new PlayerEvents.Attack();
                attack.direction = playerMovementScript.GetFacingDirection();
                PlayerEvents.OnPlayerAttack.Invoke(attack);
            }
        } 
    }

    private IEnumerator ToggleFireball()
    {
        isFireballOnCooldown = true;
        Vector3 position = new Vector3(
            facingRight ? fireballSpawn.position.x : fireballSpawn.position.x - 2*fireballSpawn.localPosition.x,
            fireballSpawn.position.y,
            fireballSpawn.position.z);
        Fireball fb = Instantiate(fireballPrefab, position, Quaternion.identity).GetComponent<Fireball>();
        fb.SetDirection(facingRight ? Vector3.right : -Vector3.right);
        fb.combatScript = this;

        VitalEnergyManager.instance.RemoveTime(fireballVitalEnergyCost);

        yield return new WaitForSeconds(cooldownFireball);
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

    private IEnumerator ToggleChargedAttack()
    {
        playerMovementScript.speedModifier *= 0.5f;
        hasReducedSpeed = true;
        yield return new WaitForSeconds(chargeTime);
        playerMovementScript.speedModifier *= 2f;
        hasReducedSpeed = false;
        playerMovementScript.DisableMovement();
        yield return new WaitForSeconds(impactTime);
        playerMovementScript.EnableMovement();
        chargedAnimator.SetBool("Charging", false);
    }

    public void ChargedAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            chargedCoroutine = StartCoroutine(ToggleChargedAttack());
            chargedAnimator.SetBool("Charging", true);
        }
        else if (context.canceled)
        {
            if (chargedCoroutine != null)
            {
                StopCoroutine(chargedCoroutine);
                if (hasReducedSpeed)
                {
                    playerMovementScript.speedModifier *= 2f;
                }
                playerMovementScript.EnableMovement();
            }
            chargedAnimator.SetBool("Charging", false);
        }
    }

    public void DealAttackDamage(GameObject reciever)
    {
        if (playerMovementScript.getLook() < 0f)
        {
            playerMovementScript.JumpAction(pogoPower);
        }
        Destroy(reciever);
    }

    
}
