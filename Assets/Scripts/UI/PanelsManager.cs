using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PanelsManager : Singleton<PanelsManager>
{
    public GameObject[] panels;

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