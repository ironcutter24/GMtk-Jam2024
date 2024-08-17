using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
