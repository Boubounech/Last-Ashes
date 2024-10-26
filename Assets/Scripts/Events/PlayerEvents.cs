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

    // ################
    // #    ATTACK    #
    // ################
    public struct Attack
    {
        public Vector2 direction;
    }

    public static UnityEvent<Attack> OnPlayerAttack = new UnityEvent<Attack>();
    public static UnityEvent OnPlayerFinishAttack = new UnityEvent();
    public static UnityEvent OnPlayerCanAttack = new UnityEvent();

    public static UnityEvent OnPlayerHitDamageable = new UnityEvent();
}
