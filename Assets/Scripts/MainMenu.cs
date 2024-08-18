using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button startButton, commandButton, creditsButton;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        startButton.onClick.AddListener(OnBeAHero);
        commandButton.onClick.AddListener(OnCommandButton);
        creditsButton.onClick.AddListener(OnCreditsButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnBeAHero()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void OnCommandButton()
    {

    }
    private void OnCreditsButton()
    {

    }
}
