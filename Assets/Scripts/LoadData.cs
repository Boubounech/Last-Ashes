using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    void Start()
    {
        SaveManager.instance.LoadFile();
    }

}
