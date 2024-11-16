using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    public string defaultSceneToLoad;
    void Start()
    {
        SaveManager.instance.LoadFile();
        string sceneToLoad = SaveManager.instance.sceneSaved;
        if(sceneToLoad == null)
        {
            sceneToLoad = defaultSceneToLoad;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

}
