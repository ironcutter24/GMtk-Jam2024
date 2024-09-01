using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyButMovement : MonoBehaviour
{
    [SerializeField] GameObject butterfly;
    [SerializeField] float moveSpeed = 4.5f;
    [SerializeField] bool despawnOnReach = true;

    [Header("Path points")]
    [SerializeField] Transform pointA;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MoveToDestination();
        }
    }

    private void MoveToDestination()
    {
        transform.DOMove(pointA.position, moveSpeed)
            .SetSpeedBased()
            .OnComplete(OnTweenComplete);
    }

    private void OnTweenComplete()
    {
        if (despawnOnReach)
        {
            butterfly.SetActive(false);
        }
    }
}
