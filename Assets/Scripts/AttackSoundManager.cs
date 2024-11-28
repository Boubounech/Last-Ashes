using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundManager : MonoBehaviour
{
    public AudioSource normalAttack;
    public AudioSource chargedAttack;
    public AudioSource fireball;
    public AudioSource dash;

    public void Awake()
    {
        PlayerEvents.OnPlayerAttack.AddListener(delegate { PlaySound(normalAttack); });
        PlayerEvents.OnPlayerReleaseChargeAttack.AddListener(delegate { PlaySound(chargedAttack); });
        PlayerEvents.OnPlayerLaunchFireball.AddListener(delegate { PlaySound(fireball); });
        PlayerEvents.OnPlayerDash.AddListener(delegate { PlaySound(dash); });

    }

    private void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); 
            audioSource.Play();
        }
    }

}
