using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    private bool isCollected = false;
    [SerializeField] private float timeBonus;
    [SerializeField] private string itemID;

    private void Awake()
    {
        PlayerEvents.OnDestroyItem.AddListener(CollectItem);
    }


    void Start()
    {
        if (InventoryManager.instance.GetCollectedItems().Contains(itemID))
        {
            isCollected = true;
        }
        if (isCollected)
        {
            Destroy(this.gameObject);
        }
    }

    private void CollectItem(GameObject itemToDestroy)
    {
        if(itemToDestroy == this.gameObject)
        {
            InventoryManager.instance.AddItemToCollected(this.itemID, timeBonus);
            isCollected = true;
            Destroy(this.gameObject);
        }
    }
}
