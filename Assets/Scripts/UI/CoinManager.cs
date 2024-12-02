using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject leftCoinHolder;
    public GameObject rightCoinHolder;

    private void Awake()
    {
        GameEvents.OnPlayerObtainLeftCoin.AddListener(delegate
        {
            leftCoinHolder.SetActive(true);
        });
        GameEvents.OnPlayerObtainRightCoin.AddListener(delegate
        {
            rightCoinHolder.SetActive(true);
        });
    }

    private void Start()
    {
        if (GameManager.hasLeftCoinPart)
        {
            leftCoinHolder.SetActive(true);
        } else
        {
            leftCoinHolder.SetActive(false);
        }
        if (GameManager.hasRightCoinPart)
        {
            rightCoinHolder.SetActive(true);
        } else
        {
            rightCoinHolder.SetActive(false);
        }
    }
}
