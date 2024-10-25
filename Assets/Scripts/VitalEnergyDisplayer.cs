using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalEnergyDisplayer : MonoBehaviour
{
    [SerializeField] private Slider vitalEnergySlider;

    private void Update()
    {
        vitalEnergySlider.value = VitalEnergyManager.instance.GetCurrentEnergyPercent();
    }
}
