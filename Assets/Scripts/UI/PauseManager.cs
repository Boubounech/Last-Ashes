using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button selectedButton;

    private void Awake()
    {
        GameEvents.OnPauseGame.AddListener(ShowPauseMenu);
        GameEvents.OnResumeGame.AddListener(HidePauseMenu);
    }

    private void Start()
    {
        HidePauseMenu();
    }

    private void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        selectedButton.Select();
    }

    private void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        GameEvents.OnResumeGame.Invoke();
    }

    public void QuitToMainMenu()
    {
        GameEvents.OnResumeGame.Invoke();
        SceneManager.LoadScene("MainMenu");
    }
}
