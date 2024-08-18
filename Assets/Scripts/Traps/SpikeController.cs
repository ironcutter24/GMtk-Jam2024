using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : GeneralObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.state == GameManager.GameState.Play)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().Death();
            }
        }
    }

    protected override void ResetState()
    {
        // Noting to reset
    } 
}
