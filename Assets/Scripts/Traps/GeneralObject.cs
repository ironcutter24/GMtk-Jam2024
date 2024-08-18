using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralObject : MonoBehaviour
{
    protected virtual void Start()
    {
        GameManager.PlayerDied += ResetState;
    }

    protected virtual void OnDestroy()
    {
        GameManager.PlayerDied -= ResetState;
    }

    protected abstract void ResetState();
}
