using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPassage : MonoBehaviour
{
    [SerializeField] private GameObject render;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            render.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            render.SetActive(true);
    }
}
