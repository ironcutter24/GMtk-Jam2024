using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    public static PanelsManager Instance;

    public GameObject[] panels;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Opens a panel, specified through name.
    /// </summary>
    /// <param name="name"></param>
    public void Open_Panel(string name)
    {
        GameObject p = Array.Find(panels, panel => panel.name == name);
        if (p == null)
        {
            Debug.LogWarning("Panel: " + name + " not found!");
            return;
        }
        p.SetActive(true);
    }

    /// <summary>
    /// Close a panel, specified through name.
    /// </summary>
    /// <param name="name"></param>
    public void Close_Panel(string name)
    {
        GameObject p = Array.Find(panels, panel => panel.name == name);
        if (p == null)
        {
            Debug.LogWarning("Panel: " + name + " not found!");
            return;
        }
        p.SetActive(false);
    }
}