using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrap : MonoBehaviour
{
    const float AMMO_SPEED = 12f;

    [SerializeField] float circleRadius = 0.5f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] LayerMask layerMask;

    [SerializeField] GameObject ammoPrefab;

    [SerializeField] float fireRate = 3f;

    private bool playerDetected;

    private bool IsFlipped => transform.localScale.x < 0f;
    private Vector3 ShootDirection => Vector3.right * (IsFlipped ? -1f : 1f);

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            CheckPlayerInRange();
        }
    }

    private void CheckPlayerInRange()
    {
        RaycastHit2D hitPlayer = Physics2D.CircleCast(transform.position, circleRadius, ShootDirection, maxDistance, layerMask);

        if (hitPlayer.collider != null)
        {
            if (playerDetected == false)
            {
                playerDetected = true;

                StartCoroutine(_ShootArrow());
            }
        }
    }

    IEnumerator _ShootArrow()
    {
        GameObject ammo = Instantiate(ammoPrefab, transform.position + ShootDirection, Quaternion.identity);
        ammo.GetComponent<Rigidbody2D>().velocity = ShootDirection * AMMO_SPEED;

        if (IsFlipped)
        {
            ammo.GetComponent<SpriteRenderer>().flipX = IsFlipped;
        }

        yield return new WaitForSeconds(fireRate);

        playerDetected = false;
    }
}
