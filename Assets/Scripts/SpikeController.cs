using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : GeneralObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ReloadCurrentLevel();
        }
    }

    /// <summary>
    /// Vado in Game Over.
    /// </summary>
    public override void GameOver()
    {
        Debug.Log("Fa qualcosa specifico per le punte quando vai in Game Over");
    } 
}
