using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] LevelList_SO levelList;
    [SerializeField] PlayerAnimControllers_SO animControllers;

    [Header("Game state")]
    [field: SerializeField] public GameState State = GameState.None;

    [Header("UI Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject statsSetPanel;
    [SerializeField] GameObject statsLockPanel;

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
        if (currentScene.name.Contains("Tutorial"))
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
            case GameState.Play:
                Time.timeScale = 1;

                HideAllUI();
                statsSetPanel.SetActive(true);
                statsLockPanel.SetActive(true);

                break;

            case GameState.Tutorial:
                Time.timeScale = 1;
                HideAllUI();
                break;

            case GameState.GameOver:
                AudioManager.Instance.PlayGameOver();
                PlayerDied?.Invoke();

                HideAllUI();
                gameOverPanel.SetActive(true);

                break;

            case GameState.PauseMenu:
                Time.timeScale = 0;

                HideAllUI();

                break;

            case GameState.BuildYourBuild:
                HideAllUI();
                statsSetPanel.SetActive(true);

                break;

            default:
                Time.timeScale = 1;
                HideAllUI();
                break;
        }
    }

    private void HideAllUI()
    {
        gameOverPanel.SetActive(false);
        statsSetPanel.SetActive(false);
        statsLockPanel.SetActive(false);
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
        Tutorial,
        Play,
        GameOver,
        PauseMenu,
        BuildYourBuild
    }
}
