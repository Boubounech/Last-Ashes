using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPower : MonoBehaviour
{

    private bool isCollected = false;
    [SerializeField] private string itemID;
    public enum PowerUpType
    {
        Dash,
        WallJump,
        Dive,
        Fireball,
        DoubleJump
    }
    public PowerUpType powerUpType;


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
        if (itemToDestroy == this.gameObject)
        {
            InventoryManager.instance.AddItemToCollected(this.itemID, 0);
            isCollected = true;
            switch (powerUpType)
            {
                case PowerUpType.Dash:
                    PlayerEvents.OnDashObtained.Invoke();
                    break;
                case PowerUpType.WallJump:
                    PlayerEvents.OnWallJumpObtained.Invoke();
                    break;
                case PowerUpType.Dive:
                    PlayerEvents.OnDiveObtained.Invoke();
                    break;
                case PowerUpType.Fireball:
                    PlayerEvents.OnFireballObtained.Invoke();
                    break;
                case PowerUpType.DoubleJump:
                    PlayerEvents.OnDoubleJumpObtained.Invoke();
                    break;
            }

            Destroy(this.gameObject);
        }


    }
}
