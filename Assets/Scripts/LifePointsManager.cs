using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePointsManager : MonoBehaviour
{
    [SerializeField] private int maxHpPoints;
    private int currentHp;

    public static LifePointsManager instance;

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
    }

    public void LoseHp()
    {
        currentHp--;
        Debug.Log(currentHp);
        if(currentHp == 0)
        {
            Debug.Log("mort");
        }
    }

    public int GetHp()
    {
        return this.currentHp;
    }
}
