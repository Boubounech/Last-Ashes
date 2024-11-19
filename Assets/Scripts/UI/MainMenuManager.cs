using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private Button selectedMainMenuButton;
    [SerializeField] private Button selectedOptionsMenuButton;

    public string initScene;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        selectedMainMenuButton.Select();
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        selectedOptionsMenuButton.Select();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(initScene);
    }

    public void DeleteSave()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "LastAshesSave.json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Fichier de sauvegarde supprimé");
        }
        else
        {
            Debug.LogWarning($"le fichier {filePath} n'existe pas");
        }
    }
}
