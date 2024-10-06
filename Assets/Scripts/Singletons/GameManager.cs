using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerAnimControllers_SO animControllers;

    [Header("Game state")]
    [field: SerializeField] public GameState State = GameState.None;

    [Header("UI Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject statsSetPanel;
    [SerializeField] GameObject statsLockPanel;

    [Header("Audio")]
    [SerializeField] StudioEventEmitter menuAndTutorialMusic;
    [SerializeField] StudioEventEmitter dungeonMusic;

    public PlayerController PlayerController { get; private set; }
    public PlayerAnimControllers_SO AnimControllers => animControllers;

    public static event Action PlayerDied;


    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        RefreshStateForCurrentScene();

        SceneManager.sceneLoaded += (t, s) => RefreshStateForCurrentScene();
    }

    private void RefreshStateForCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Contains("MainMenu"))
        {
            SetGameState(GameState.Menu);
        }
        else if (currentScene.name.Contains("Tutorial"))
        {
            SetGameState(GameState.Tutorial);
        }
        else if (currentScene.name.Contains("Level"))
        {
            SetGameState(GameState.BuildYourBuild);
        }
        else
        {
            SetGameState(GameState.None);
        }
    }

    public void SetPlayerController(PlayerController playerController)
    {
        PlayerController = playerController;
    }

    public void UnsetPlayerController(PlayerController playerController)
    {
        if (PlayerController == playerController)
        {
            PlayerController = null;
        }
    }

    public void SetGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Menu:
                Time.timeScale = 1;
                ShowCursor();
                HideAllUI();
                TryPlayMenuAndDungeonMusic();
                break;

            case GameState.Tutorial:
                Time.timeScale = 1;
                HideCursor();
                HideAllUI();
                TryPlayMenuAndDungeonMusic();
                break;

            case GameState.BuildYourBuild:
                Time.timeScale = 1;

                // UI -----------------------------------
                ShowCursor();
                HideAllUI();
                statsSetPanel.SetActive(true);

                // Audio --------------------------------
                TryPlayDungeonMusic();

                break;

            case GameState.Play:
                Time.timeScale = 1;

                // UI -----------------------------------
                HideCursor();
                HideAllUI();
                statsSetPanel.SetActive(false);
                statsLockPanel.SetActive(true);

                break;

            case GameState.GameOver:
                PlayerDied?.Invoke();

                // UI -----------------------------------
                ShowCursor();
                HideAllUI();
                gameOverPanel.SetActive(true);

                break;

            case GameState.PauseMenu:
                Time.timeScale = 0;

                // UI -----------------------------------
                ShowCursor();
                HideAllUI();

                break;

            default:
                Time.timeScale = 1;

                // UI -----------------------------------
                HideCursor();
                HideAllUI();

                // Audio --------------------------------
                StopAllMusic();

                break;
        }
    }

    private void TryPlayMenuAndDungeonMusic()
    {
        if (!menuAndTutorialMusic.IsPlaying())
        {
            menuAndTutorialMusic.Play();
        }
        dungeonMusic.Stop();
    }

    private void TryPlayDungeonMusic()
    {
        if (!dungeonMusic.IsPlaying())
        {
            dungeonMusic.Play();
        }
        menuAndTutorialMusic.Stop();
    }

    private void StopAllMusic()
    {
        menuAndTutorialMusic.Stop();
        dungeonMusic.Stop();
    }

    private void HideAllUI()
    {
        gameOverPanel.SetActive(false);
        statsSetPanel.SetActive(false);
        statsLockPanel.SetActive(false);
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReloadCurrentLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("No more levels to load!");
        }
    }

    public enum GameState
    {
        None,
        Menu,
        Tutorial,
        Play,
        GameOver,
        PauseMenu,
        BuildYourBuild
    }
}
