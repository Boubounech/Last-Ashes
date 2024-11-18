using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossZone : MonoBehaviour
{
    public GameObject angelBoss;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        string id = angelBoss.GetComponent<AngelBoss>().GetAngelId();
        bool isAlreadyBeaten = SaveManager.instance.bossesSaved.Contains(id) || SaveManager.instance.bossesToSave.Contains(id);
        if (!isAlreadyBeaten)
        {
            if (collision.CompareTag("Player"))
            {
                GameEvents.OnPlayerEnterBossZone.Invoke();
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("already beaten");
        }


    }
}
