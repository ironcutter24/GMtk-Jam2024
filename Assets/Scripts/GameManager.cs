using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : Singleton<GameManager>
{
    private int levelIndex = 0;


    [SerializeField] LevelList_SO levelList;

    [Header("Game state:")]
    [SerializeField] GameState state;

    public PlayerController PlayerController { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetPlayerController(PlayerController playerController)
    {
        PlayerController = playerController;
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Play:
                break;
            case GameState.GameOver:
                break;
            case GameState.PauseMenu:
                break;
            case GameState.BuildYourBuild:
                break;
        }
    }

    public void ReloadCurrentLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name, LoadSceneMode.Single);
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
