using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnableDive : MonoBehaviour
{
    public BoxCollider2D playerHitbox;
    public BoxCollider2D playerColisionBox;
    public GameObject playerRender;
    public GameObject inDivePlayerRender;

    public GameObject explosionPrefab;
    public Combat combatScript;

    [SerializeField] private float invicibilityTime;
    [SerializeField] private float cooldownDive;
    [SerializeField] private bool isSceneChanging;

    private void Awake()
    {
        PlayerEvents.OnPlayerDive.AddListener(StartDive);
        PlayerEvents.OnPlayerDiveEnd.AddListener(EndDive);
        PlayerEvents.OnPlayerChangeScene.AddListener(delegate { isSceneChanging = true; });
        inDivePlayerRender.SetActive(false);
        isSceneChanging = false;
    }

    public void StartDive()
    {
        playerHitbox.enabled = false;
        playerColisionBox.enabled = false;
        playerRender.SetActive(false);
        inDivePlayerRender.SetActive(true);
    }

    public void EndDive()
    {

        playerColisionBox.enabled = true;
        playerRender.SetActive(true);
        inDivePlayerRender.SetActive(false);
        if (!isSceneChanging) // pour eviter une erreur au moment de changer de scene
        {
            DetectContactAttack explosionAttack = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<DetectContactAttack>();
            explosionAttack.combatScript = combatScript;
        }
        Invoke("RemoveInvicibility", invicibilityTime);
        Invoke("AllowDive", cooldownDive);


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
