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
    private bool canAttack = true;

    [Header("Fireball")]
    private bool isFireballOnCooldown;
    private bool canFirball = true;

    [Header("ChargedAttack")]
    private bool isChargingAttack = false;
    private bool isCharged = false;
    private bool canChargedAttack = true;
    private bool canFinishChargedAttack = false;

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
            playerMovementScript.speedModifier = 0.5f;
        });
        PlayerEvents.OnPlayerInterruptChargeAttack.AddListener(delegate { 
            isChargingAttack = false;
            playerMovementScript.speedModifier = playerMovementScript.baseSpeeModifier;
        });
        PlayerEvents.OnPlayerFinishChargeAttack.AddListener(delegate { 
            isChargingAttack = false; 
            isCharged = false;
            playerMovementScript.speedModifier = playerMovementScript.baseSpeeModifier;
        });
        PlayerEvents.OnPlayerDive.AddListener(delegate
        {
            canAttack = false;
            canChargedAttack = false;
            canFirball = false;
            if(isChargingAttack)
            {
                PlayerEvents.OnPlayerInterruptChargeAttack.Invoke();
            }
            isCharged = false;
        });
        PlayerEvents.OnPlayerDiveEnd.AddListener(delegate
        {
            canAttack = true;
            canChargedAttack = true;
            canFirball = true;
        });
        PlayerEvents.OnPlayerChargeChargeAttack.AddListener (delegate { isCharged = true; });
    }

    private void Start()
    {
        facingRight = playerMovementScript.getFacing();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
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
        if (context.performed && canFirball)
        {
            if (!isFireballOnCooldown)
            {
                PlayerEvents.OnPlayerLaunchFireball.Invoke(playerMovementScript.getFacing() ? 1 : -1);
            }

        }
    }

    public void ChargedAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canChargedAttack)
        {
            PlayerEvents.OnPlayerChargeAttack.Invoke(playerMovementScript.getFacing() ? 1 : -1);
            canFinishChargedAttack = true; // pour éviter que l'on puisse finir sans l'avoir commencer (restriction a cause du dive)
        }
        else if (context.canceled && canFinishChargedAttack) 
        {
            if (isCharged)
            {
                PlayerEvents.OnPlayerReleaseChargeAttack.Invoke();
            } else if (isChargingAttack)
            {
                PlayerEvents.OnPlayerInterruptChargeAttack.Invoke();
            }
            canFinishChargedAttack = false;
        }
    }

    public void PogoOnDamage()
    {
        if (playerMovementScript.getLook() < 0f)
        {
            playerMovementScript.JumpAction(pogoPower);
        }
    }
}
