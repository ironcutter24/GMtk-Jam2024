using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GameManager : Singleton<GameManager>
{
    //Variables:
    [Header("Game state:")]
    public GameState state;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
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

    public enum GameState
    {
        Play,
        GameOver,
        PauseMenu,
        BuildYourBuild
    }
}
