using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    private static bool isCollected = false;
    [SerializeField] private float timeBonus;

    private void Awake()
    {
        PlayerEvents.OnDestroyItem.AddListener(CollectItem);
    }


    void Start()
    {
        if (isCollected)
        {
            Destroy(gameObject);
        }
    }

    private void CollectItem(GameObject itemToDestroy)
    {
        if(itemToDestroy == this.gameObject)
        {
            InventoryManager.instance.AddItemToInventory(this.gameObject, timeBonus);
            isCollected = true;
            Destroy(this.gameObject);
        }
    }
}
