using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : GeneralObject
{
    [SerializeField] bool alwaysKillPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            if (collision.CompareTag("Player"))
            {
                if (PlayerStats.Instance.MoveSpeed >= 1 || alwaysKillPlayer)
                {
                    collision.GetComponent<PlayerController>().Death(DeathType.Default);
                }
            }
        }
    }

    protected override void ResetState()
    {
        // Noting to reset
    }
}
