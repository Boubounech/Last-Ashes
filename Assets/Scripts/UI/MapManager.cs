using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject mapImg;

    private void Awake()
    {
        GameEvents.OnMapOpened.AddListener(OpenMap);
        GameEvents.OnMapClosed.AddListener(CloseMap);
    }

    private void Start()
    {
        mapImg.SetActive(false);
    }

    private void OpenMap()
    {
        mapImg.SetActive(true);
    }

    private void CloseMap()
    {
        mapImg.SetActive(false);
    }
}
