using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button restartButton, quitButton;
    [SerializeField] PlayerController player;

    private void Start()
    {
        gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButton);
        quitButton.onClick.AddListener(OnQuitButton);
    }

    public void Died()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;  // Pause game
        
    }
    private void OnRestartButton()
    {
        gameObject.SetActive(false);
        player.Death();
    }

    private void OnQuitButton()
    {
        Debug.Log("Quit button pressed!");
        Application.Quit();
    }
}