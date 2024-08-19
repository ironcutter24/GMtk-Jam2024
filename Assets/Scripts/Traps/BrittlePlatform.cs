using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittlePlatform : MonoBehaviour
{
    const float BREAK_WAIT_TIME = 1f;
    const float BREAK_DURATION_TIME = 1f;

    [SerializeField, Range(0, 2)] int maxSuportedWeight = 2;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] BoxCollider2D boxCollider;

    private bool wasTriggered = false;

    public bool IsExcedingMaxWeight => PlayerStats.Instance.Weight >= maxSuportedWeight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasTriggered) return;

        if (collision.CompareTag("Player") || collision.CompareTag("Crate"))
        {
            if (!IsExcedingMaxWeight) return;

            wasTriggered = true;

            Sequence mySequence = DOTween.Sequence();
            mySequence
                .Append(
                    spriteRenderer.transform.DOShakePosition(duration: .4f, strength: .12f, vibrato: 10)
                )
                .Insert(BREAK_WAIT_TIME,
                    spriteRenderer.DOFade(0f, BREAK_DURATION_TIME)
                )
                .Insert(BREAK_WAIT_TIME,
                    spriteRenderer.transform.DOScale(0f, BREAK_DURATION_TIME)
                )
                .InsertCallback(BREAK_WAIT_TIME + BREAK_DURATION_TIME * .3f,
                    DisableCollider
                );
        }
    }

    private void DisableCollider()
    {
        boxCollider.enabled = false;
    }
}
