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
        BuildToBuiltMoment();
    }

    public void BuildToBuiltMoment()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.BuildYourBuild);
        PanelsManager.Instance.Open_Panel("BuildYourBuild_Panel");
        PanelsManager.Instance.Close_Panel("LockStat_Panel");
    }

    public void ConfirmYourBuild()
    {
        PanelsManager.Instance.Close_Panel("BuildYourBuild_Panel");
        PanelsManager.Instance.Open_Panel("LockStat_Panel");

        GameManager.Instance.SetGameState(GameManager.GameState.Play);
    }
}
