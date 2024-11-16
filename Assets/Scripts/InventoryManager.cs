using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private float timeBonusToAdd = 0;
    private List<string> collectedItems = new List<string>(); 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToCollected(string itemID, float timeBonus)
    {
        collectedItems.Add(itemID);
        timeBonusToAdd += timeBonus;
        SaveManager.instance.itemsToSave.Add(itemID);
    }

    public void GainBonusFromItems()
    {
        VitalEnergyManager.instance.AddMaxEnergyTime(timeBonusToAdd);
        timeBonusToAdd = 0;
        
    }

    public List<string> GetCollectedItems()
    {
        return collectedItems;
    }

    public void SetCollectedItems(List<string> newCollectedItems)
    {
        this.collectedItems = newCollectedItems;
    }
}
