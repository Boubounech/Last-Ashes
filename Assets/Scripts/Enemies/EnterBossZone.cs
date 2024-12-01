using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossZone : MonoBehaviour
{
    public GameObject angelBoss;
    public GameObject marionnettiste;
    private string id;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(angelBoss != null)
        {
            id = angelBoss.GetComponent<AngelBoss>().GetAngelId();
        }
        else
        {
            id = marionnettiste.GetComponent<Marionnettiste>().GetId();
        }

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
