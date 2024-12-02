using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    public bool isPlayerInRange;
    public bool isOpen;
    public Animator animator;
    public string lastSceneName = "LastScene";

    private void Awake()
    {
        GameEvents.OnPlayerTryToOpenDoor.AddListener(TryToOpen);
    }

    private void TryToOpen()
    {
        if (isPlayerInRange && !isOpen)
        {
            if (GameManager.hasLeftCoinPart && GameManager.hasRightCoinPart)
            {
                animator.SetTrigger("Open");
            }
        } else if (isPlayerInRange && isOpen)
        {
            SceneManager.LoadScene(lastSceneName);
        }
    }

    public void DoorOpened()
    {
        isOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
