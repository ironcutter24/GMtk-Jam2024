using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuiltToBuilt_Manager : MonoBehaviour
{
    [SerializeField] Button builtToBuilt_Button;

    private void Start()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.BuildYourBuild);

        PanelsManager.Instance.Open_Panel("BuiltToBuilt_Panel");
    }

    public void ConfirmYourBuild()
    {
        PanelsManager.Instance.Close_Panel("BuiltToBuilt_Panel");
        PanelsManager.Instance.Open_Panel("LockStat_Panel");

        GameManager.Instance.SetGameState(GameManager.GameState.Play);
    }
}
