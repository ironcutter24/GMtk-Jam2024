using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton, restartButton, quitButton;
    [SerializeField] bool isPaused = false;

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButton);
        restartButton.onClick.AddListener(OnRestartButton);
        quitButton.onClick.AddListener(OnQuitButton);

        InputManager.Actions.Player.PauseGame.performed += PlayerTogglePause_performed;
        InputManager.Actions.UI.Cancel.performed += PlayerTogglePause_performed;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        InputManager.Actions.Player.PauseGame.performed -= PlayerTogglePause_performed;
        InputManager.Actions.UI.Cancel.performed -= PlayerTogglePause_performed;
    }

    private void PlayerTogglePause_performed(InputAction.CallbackContext context)
    {
        if (isPaused) OnResumeButton();
        else PauseGame();
    }

    private void PauseGame()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;  // Pause game
        isPaused = true;
    }

    private void OnResumeButton()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;  // Resume game
        isPaused = false;
    }

    private void OnRestartButton()
    {
        GameManager.Instance.ReloadCurrentLevel();
        OnResumeButton();
    }

    private void OnQuitButton()
    {
        Debug.Log("Quit button pressed!");
        Application.Quit();
    }
}
