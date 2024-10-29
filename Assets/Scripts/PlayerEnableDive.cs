using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnableDive : MonoBehaviour
{
    public GameObject explosionHitbox;
    public BoxCollider2D playerHitbox;
    public BoxCollider2D playerColisionBox;
    public GameObject playerRender;
    public GameObject diveRender;

    [SerializeField] private float explosionTime;
    [SerializeField] private float invicibilityTime;
    [SerializeField] private float cooldownDive;

    private void Awake()
    {
        PlayerEvents.OnPlayerDive.AddListener(StartDive);
        PlayerEvents.OnPlayerDiveEnd.AddListener(EndDive);
        explosionHitbox.SetActive(false);
        diveRender.SetActive(false);
    }

    public void StartDive()
    {
        playerHitbox.enabled = false;
        playerColisionBox.enabled = false;
        playerRender.SetActive(false);
        diveRender.SetActive(true);
    }

    public void EndDive()
    {

        playerColisionBox.enabled = true;
        playerRender.SetActive(true);
        diveRender.SetActive(false);

        explosionHitbox.SetActive(true);
        Invoke("RemoveExplosion", explosionTime);
        Invoke("RemoveInvicibility", invicibilityTime);
        Invoke("AllowDive", cooldownDive);

    }

    private void RemoveExplosion()
    {
        explosionHitbox.SetActive(false);
    }

    private void RemoveInvicibility()
    {
        playerHitbox.enabled = true;
    }

    private void AllowDive()
    {
        PlayerEvents.OnPlayerCanDive.Invoke();
    }
   


}
