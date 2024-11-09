using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private float timeBonusToAdd = 0;
    private HashSet<string> collectedItems = new HashSet<string>(); //potentiellement pour l'affichage, a supprimer sinon

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
        collectedItems.Add(itemID); //potentiellement pour l'affichage, a supprimer sinon
        timeBonusToAdd += timeBonus;
    }

    public void GainBonusFromItems()
    {
        VitalEnergyManager.instance.AddMaxEnergyTime(timeBonusToAdd);
        timeBonusToAdd = 0;
    }

    public HashSet<string> GetCollectedItems()
    {
        return collectedItems;
    }
}
