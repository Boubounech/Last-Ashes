using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnableDive : MonoBehaviour
{
    public GameObject explosionHitbox;
    public BoxCollider2D playerHitbox;
    public BoxCollider2D playerColisionBox;
    public GameObject playerRender;
    public SpriteRenderer diveRender;


    [SerializeField] private float explosionTime;

    private void Awake()
    {
        PlayerEvents.OnPlayerDive.AddListener(StartDive);
        PlayerEvents.OnPlayerDiveEnd.AddListener(EndDive);
        explosionHitbox.SetActive(false);
        diveRender.enabled = false;
    }

    public void StartDive()
    {
        playerHitbox.enabled = false;
        playerColisionBox.enabled = false;
        playerRender.SetActive(false);
        diveRender.enabled = true;
    }

    public void EndDive()
    {
        playerHitbox.enabled = true;
        playerColisionBox.enabled = true;
        playerRender.SetActive(true);
        diveRender.enabled = false;

        explosionHitbox.SetActive(true);
        Invoke("RemoveExplosion", explosionTime);
    }

    private void RemoveExplosion()
    {
        explosionHitbox.SetActive(false);
    }
   


}
