using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static GameManager;

public class GameOverUI : MonoBehaviour
{
    public void On_RestartButton()
    {
        GameManager.Instance.ReloadCurrentLevel();
        PanelsManager.Instance.Close_Panel("GameOver_Panel");
        GameManager.Instance.SetGameState(GameState.Play);
    }

    public void On_QuitButton()
    {
        Debug.Log("Quit button pressed!");
        Application.Quit();
    }
}