using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuIn : MonoBehaviour
{
    public float timer;

    void Start()
    {
        Invoke("Return", timer);
    }

    public void Return()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
