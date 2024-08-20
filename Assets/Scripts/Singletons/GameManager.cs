using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : Singleton<GameManager>
{
    private int levelIndex = 0;


    [SerializeField] LevelList_SO levelList;
    [SerializeField] PlayerAnimControllers_SO animControllers;

    [Header("Game state:")]
    public GameState state;

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
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene == SceneManager.GetSceneByName("MainMenu"))
        {
            PanelsManager.Instance.Close_Panel("SliderPanel_Script");
            PanelsManager.Instance.Close_Panel("BuildYourBuild_Panel");
        }
        else
        {
            PanelsManager.Instance.Open_Panel("SliderPanel_Script");
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
        state = newState;

        switch (newState)
        {
            case GameState.Play:
                Time.timeScale = 1;
                break;

            case GameState.GameOver:
                AudioManager.Instance.PlayGameOver();
                PlayerDied?.Invoke();
                break;

            case GameState.PauseMenu:
                Time.timeScale = 0;
                break;

            case GameState.BuildYourBuild:
                break;
        }
    }

    public void ReloadCurrentLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name, LoadSceneMode.Single);

        gameObject.GetComponent<BuildYourBuild_Manager>().BuildToBuiltMoment();
    }

    public void LoadFirstLevel()
    {
        levelIndex = 0;
        SceneManager.LoadScene(levelList.GetLevelAt(levelIndex), LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        if (++levelIndex >= levelList.GetCount())
        {
            Debug.LogError("Level index has gone over level count.");
            levelIndex--;
            return;
        }

        var sceneName = levelList.GetLevelAt(levelIndex);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public enum GameState
    {
        Play,
        GameOver,
        PauseMenu,
        BuildYourBuild
    }
}
