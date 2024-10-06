using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallengerKnight : MonoBehaviour
{
    [SerializeField] float speedAttack = 1f;

    Animator animator;
    bool playerIsNear;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("ChallengerKnight_Idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            if (collision.CompareTag("Player"))
            {
                playerIsNear = true;

                if (PlayerStats.Instance.Strength == 0)
                {
                    animator.Play("ChallengerKnight_Attack");

                    StartCoroutine(GloveAttack(collision));
                }
                else
                {
                    return;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            if (collision.CompareTag("Player"))
            {
                animator.Play("ChallengerKnight_Idle");
                playerIsNear = false;
            }
        }
    }


    IEnumerator GloveAttack(Collider2D collision)
    {
        yield return new WaitForSeconds(speedAttack);

        if (playerIsNear)
        {
            collision.GetComponent<PlayerController>().Death(DeathType.Knight);
        }
        else
        {
            StopCoroutine(GloveAttack(collision));
        }
    }
}
