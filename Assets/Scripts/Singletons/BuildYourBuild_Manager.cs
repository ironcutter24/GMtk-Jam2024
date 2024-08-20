using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildYourBuild_Manager : MonoBehaviour
{
    [SerializeField] Button confirmYourBuild_Button;

    private void Start()
    {
        confirmYourBuild_Button.onClick.AddListener(ConfirmYourBuild);
    }

    public void ConfirmYourBuild()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Play);
    }
}
