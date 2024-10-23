using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalEnergieManager : MonoBehaviour
{
    [SerializeField] private float maxEnergieTime;
    private float timeSpeedMultiplicator = 1f;
    private float currentEnergieTime;
    private bool stopTimer;

    public static VitalEnergieManager instance;

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
            if (currentEnergieTime >= 0)
            {
                currentEnergieTime -= Time.deltaTime * timeSpeedMultiplicator;
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
        currentEnergieTime -= removeTime;
    }

    public void ResetTimer()
    {
        currentEnergieTime = maxEnergieTime;
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
}
