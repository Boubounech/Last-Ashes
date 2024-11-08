using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private float timeBonusToAdd = 0;
    private HashSet<GameObject> collectedItems = new HashSet<GameObject>(); //potentiellement pour l'affichage, a supprimer sinon

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

    public void AddItemToInventory(GameObject item, float timeBonus)
    {
        collectedItems.Add(item); //potentiellement pour l'affichage, a supprimer sinon
        timeBonusToAdd += timeBonus;
    }

    public void GainBonusFromItems()
    {
        VitalEnergyManager.instance.AddMaxEnergyTime(timeBonusToAdd);
        timeBonusToAdd = 0;
        collectedItems.Clear(); //potentiellement pour l'affichage, a supprimer sinon
    }


}
