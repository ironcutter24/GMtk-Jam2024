using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button controlsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button homeButton2;

    [Header("Panels")]
    [SerializeField] Transform controlsPanel;
    [SerializeField] Transform creditsPanel;


    private void Start()
    {
        gameObject.SetActive(true);
        startButton.onClick.AddListener(OnBeAHeroButton);
        controlsButton.onClick.AddListener(OnControlsButton);
        creditsButton.onClick.AddListener(OnCreditsButton);
        quitButton.onClick.AddListener(OnQuit);
        homeButton.onClick.AddListener(OnBack);
        homeButton2.onClick.AddListener(OnBack);
        InputManager.Actions.UI.Cancel.performed += UICancel_performed;
        InputManager.SwitchActionMapToUI();
    }

    private void OnDestroy()
    {
        InputManager.Actions.UI.Cancel.performed -= UICancel_performed;
    }

    private void UICancel_performed(InputAction.CallbackContext context)
    {
        OnBack();
    }

    private void OnBeAHeroButton()
    {
        AudioManager.Instance.PlayUIGameStarted();
        GameManager.Instance.LoadNextLevel();
    }
    private void OnControlsButton()
    {
        controlsPanel.gameObject.SetActive(true);
    }
    private void OnCreditsButton()
    {
        creditsPanel.gameObject.SetActive(true);
    }
    private void OnQuit()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.LogWarning("Running in WebGL: Cannot quit application.");
            return;
        }

        Application.Quit();
        Debug.Log("Game is quitting"); // This will not be seen in the build
    }

    private void OnBack()
    {
        controlsPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(false);
        AudioManager.Instance.PlayUICancel();
    }
}
