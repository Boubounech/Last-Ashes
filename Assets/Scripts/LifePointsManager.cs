using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifePointsManager : MonoBehaviour
{
    [SerializeField] private int maxHpPoints;
    [SerializeField] private int currentHp;

    public static LifePointsManager instance;

    public static UnityEvent<int> OnHpChanged = new UnityEvent<int>();

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

    void Start()
    {
        SetHpTo(maxHpPoints);
    }

    public void SetHpTo(int hp)
    {
        this.currentHp = hp;
        OnHpChanged.Invoke(hp);
    }

    public void LoseHp()
    {
        SetHpTo(currentHp - 1);
        if(currentHp == 0)
        {
            //mort 
        }
    }

    public void GainHp()
    {
        SetHpTo(currentHp + 1);
    }

    public int GetHp()
    {
        return this.currentHp;
    }
    public int GetMaxHp()
    {
        return this.maxHpPoints;
    }
}
