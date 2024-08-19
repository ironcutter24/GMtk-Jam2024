using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [Header("Path points")]
    [SerializeField] Transform pointA;

    private void Start()
    {
        StartMovingBlade();
    }

    private void StartMovingBlade()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(transform.DOMove(pointA.position, 2f).SetUpdate(UpdateType.Fixed))
            .Append(transform.DOMove(transform.parent.position, 2f).SetUpdate(UpdateType.Fixed))
            .SetLoops(-1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.state == GameManager.GameState.Play)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().Death(DeathType.Blade);
            }
        } 
    }
}
