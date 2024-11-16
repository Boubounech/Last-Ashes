using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalEnergyManager : MonoBehaviour
{
    [SerializeField] private float maxEnergyTime;
    private float timeSpeedMultiplicator = 1f;
    private float currentEnergyTime;
    private bool stopTimer;

    public static VitalEnergyManager instance;

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

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if(stopTimer == false)
        {
            if (currentEnergyTime >= 0)
            {
                currentEnergyTime -= Time.deltaTime * timeSpeedMultiplicator;
            }
            else
            {
                stopTimer = true;
                Debug.Log("mort");
            }
        }
    }

    public void RemoveTime(float removeTime)
    {
        currentEnergyTime -= removeTime;
    }

    public void ResetTimer()
    {
        currentEnergyTime = maxEnergyTime;
        stopTimer = false;
    }

    public void PauseTimer()
    {
        stopTimer = true;
    }
    
    public void ResumeTimer()
    {
        stopTimer = false;
    }

    public void ChangeSpeedTimer(float speedMult)
    {
        timeSpeedMultiplicator = speedMult;
    }

    public void AddMaxEnergyTime(float maxEnergyToAdd)
    {
        maxEnergyTime += maxEnergyToAdd;
        SaveManager.instance.timeToSave = maxEnergyTime;
    }

    public void SetMaxEnergyTime(float maxEnergyToSet)
    {
        maxEnergyTime = maxEnergyToSet;
    }

    public float GetCurrentEnergyPercent()
    {
        return currentEnergyTime / maxEnergyTime;
    }
}
