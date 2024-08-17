using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [SerializeField] float bladeSpeed;

    public bool isCoroutineRunning;

    private void Start()
    {
        StartMovingBlade();
    }

    private void StartMovingBlade()
    {
        StartCoroutine(MovingBlade());
    }

    IEnumerator MovingBlade()
    {
        isCoroutineRunning = true;

        yield return transform.DOMove(pointB.transform.position, 2f).WaitForCompletion();

        yield return transform.DOMove(pointA.transform.position, 2f).WaitForCompletion();

        isCoroutineRunning = false;

        StartCoroutine(MovingBlade());
    }
        

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Death();
        }
    }
}
