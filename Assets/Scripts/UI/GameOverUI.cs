using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button restartButton, mainMenuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(On_RestartButton);
        mainMenuButton.onClick.AddListener(On_MenuButton);
    }

    public void On_RestartButton()
    {
        GameManager.Instance.ReloadCurrentLevel();
        GameManager.Instance.SetGameState(GameState.BuildYourBuild);
    }

    public void On_MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}