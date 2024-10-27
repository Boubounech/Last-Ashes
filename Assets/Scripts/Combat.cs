using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    public PlayerMovementRays playerMovementScript;
    private bool facingRight;

    [Header("Attack")]
    public float pogoPower; // To check on OnPlayerHitDamageable
    private bool isAttackOnCooldown;

    [Header("Fireball")]
    private bool isFireballOnCooldown;

    [Header("ChargedAttack")]
    private bool isChargingAttack = false;
    private bool isCharged = false;

    private void Awake()
    {
        // Attack
        PlayerEvents.OnPlayerAttack.AddListener(delegate { isAttackOnCooldown = true; });
        PlayerEvents.OnPlayerCanAttack.AddListener(delegate { isAttackOnCooldown = false; });

        // Fireball
        PlayerEvents.OnPlayerLaunchFireball.AddListener(delegate { isFireballOnCooldown = true; });
        PlayerEvents.OnPlayerCanLaunchFireball.AddListener(delegate { isFireballOnCooldown = false; });

        // Charged Attack
        PlayerEvents.OnPlayerChargeAttack.AddListener(delegate { 
            isChargingAttack = true;
            playerMovementScript.speedModifier *= 0.5f;
        });
        PlayerEvents.OnPlayerInterruptChargeAttack.AddListener(delegate { 
            isChargingAttack = false;
            playerMovementScript.speedModifier *= 2f;
        });
        PlayerEvents.OnPlayerFinishChargeAttack.AddListener(delegate { 
            isChargingAttack = false; 
            isCharged = false;
            playerMovementScript.speedModifier *= 2f;
        });
        PlayerEvents.OnPlayerChargeChargeAttack.AddListener (delegate { isCharged = true; });

        // On hit
        PlayerEvents.OnPlayerHitDamageable.AddListener(delegate { Debug.LogWarning("To implement based on AttackHit structure"); });
    }

    private void Start()
    {
        facingRight = playerMovementScript.getFacing();
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

    public void Fireball(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isFireballOnCooldown)
            {
                PlayerEvents.OnPlayerLaunchFireball.Invoke(playerMovementScript.getFacing() ? 1 : -1);
            }

        }
    }

    public void ChargedAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerEvents.OnPlayerChargeAttack.Invoke(playerMovementScript.getFacing() ? 1 : -1);
        }
        else if (context.canceled)
        {
            if (isCharged)
            {
                PlayerEvents.OnPlayerReleaseChargeAttack.Invoke();
            } else if (isChargingAttack)
            {
                PlayerEvents.OnPlayerInterruptChargeAttack.Invoke();
            }
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
