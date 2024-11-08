using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjectContact : MonoBehaviour
{
    private bool isCloseToCampfire = false;
    private bool isCloseToItem = false;
    private GameObject lastObjectTouched;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Campfire"))
        {
            isCloseToCampfire = true;
            lastObjectTouched = other.gameObject;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            isCloseToItem = true;
            lastObjectTouched = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Campfire"))
        {
            isCloseToCampfire = false;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            isCloseToItem = false;
        }
    }

    public bool GetIsCloseToCampfire()
    {
        return this.isCloseToCampfire;
    }

    public bool GetIsCloseToItem()
    {
        return this.isCloseToItem;
    }

    public GameObject GetLastObjectTouched()
    {
        return this.lastObjectTouched;
    }

}
