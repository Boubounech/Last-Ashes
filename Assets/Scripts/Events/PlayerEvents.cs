using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents
{
    // ################
    // #   MOVEMENT   #
    // ################
    public static UnityEvent<float> OnPlayerHorizontalFacing = new UnityEvent<float>();
    public static UnityEvent<float> OnPlayerVerticalFacing = new UnityEvent<float>();
    public static UnityEvent OnPlayerDash = new UnityEvent();

    // Scene
    public static UnityEvent OnPlayerChangeScene = new UnityEvent();

    // ################
    // #    ATTACK    #
    // ################
    public struct Attack
    {
        public Vector2 direction;
    }

    

    // Attack
    public static UnityEvent<Attack> OnPlayerAttack = new UnityEvent<Attack>();
    public static UnityEvent OnPlayerFinishAttack = new UnityEvent();
    public static UnityEvent OnPlayerCanAttack = new UnityEvent();

    // Fireball
    public static UnityEvent<float> OnPlayerLaunchFireball = new UnityEvent<float>(); // right +1 of left -1
    public static UnityEvent OnPlayerCanLaunchFireball = new UnityEvent();

    // Charged attack
    public static UnityEvent<float> OnPlayerChargeAttack = new UnityEvent<float>(); // right +1 or left -1
    public static UnityEvent OnPlayerChargeChargeAttack = new UnityEvent();
    public static UnityEvent OnPlayerInterruptChargeAttack = new UnityEvent();
    public static UnityEvent OnPlayerReleaseChargeAttack = new UnityEvent();
    public static UnityEvent OnPlayerFinishChargeAttack = new UnityEvent();

    //Dive
    public static UnityEvent OnPlayerDive = new UnityEvent();
    public static UnityEvent OnPlayerDiveEnd = new UnityEvent();
    public static UnityEvent OnPlayerCanDive = new UnityEvent();

    // Hit something
    public static UnityEvent<float, GameObject> OnPlayerHitDamageable = new UnityEvent<float, GameObject>();

    //Item
    public static UnityEvent<GameObject> OnDestroyItem = new UnityEvent<GameObject>();
    public static UnityEvent OnDashObtained = new UnityEvent();
    public static UnityEvent OnWallJumpObtained = new UnityEvent();
    public static UnityEvent OnDiveObtained = new UnityEvent();
    public static UnityEvent OnFireballObtained = new UnityEvent();
    public static UnityEvent OnDoubleJumpObtained = new UnityEvent();
}
