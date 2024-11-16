using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEvents.OnPlayerEnterBossZone.Invoke();
            gameObject.SetActive(false);
        }
    }
}
