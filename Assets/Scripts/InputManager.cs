using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class InputManager : Singleton<InputManager>
{
    public static GMtk2024InputActions Actions { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            Actions = new GMtk2024InputActions();
        }
    }

    public static void SwitchActionMapToPlayer()
    {
        DisableAllActionMaps();
        Actions.Player.Enable();
    }

    public static void SwitchActionMapToUI()
    {
        DisableAllActionMaps();
        Actions.UI.Enable();
    }

    public static void DisableAllActionMaps()
    {
        Actions.Disable();
    }
}
