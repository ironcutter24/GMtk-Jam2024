using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : GeneralObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Death();
        }
    }

    protected override void ResetState()
    {
        // Noting to reset
    } 
}
