using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wasTriggered && collision.CompareTag("Player"))
        {
            wasTriggered = true;
            GameManager.Instance.LoadNextLevel();
        }
    }
}
