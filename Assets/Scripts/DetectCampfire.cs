using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCampfire : MonoBehaviour
{
    private bool isCloseToCampfire;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Campfire"))
        {
            isCloseToCampfire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Campfire"))
        {
            isCloseToCampfire = false;
        }
    }

    public bool GetIsCloseToCampfire()
    {
        return this.isCloseToCampfire;
    }
}
