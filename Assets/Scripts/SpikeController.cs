using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : GeneralObject
{
    /// <summary>
    /// Vado in Game Over.
    /// </summary>
    public override void GameOver()
    {
        base.GameOver();
        Debug.Log("Fa qualcosa specifico per le punte quando vai in Game Over");

        GameManager.Instance.UpdateGameState(GameManager.GameState.GameOver);
    } 
}
