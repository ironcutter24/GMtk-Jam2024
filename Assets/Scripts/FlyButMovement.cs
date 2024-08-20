using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyButMovement : MonoBehaviour
{
    [Header("Path points")]
    [SerializeField] Transform pointA;
    private void StartMovingBlade()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(pointA.position, 6f).SetUpdate(UpdateType.Fixed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            if (collision.CompareTag("Player"))
            {
                StartMovingBlade();
            }
        } 
    }
}
