using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrap : MonoBehaviour
{
    [SerializeField] float circleRadius = 0.5f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] LayerMask layerMask;

    [SerializeField] GameObject ammoPrefab;

    [SerializeField] float ammoSpeed = 5f;
    [SerializeField] float fireRate = 3f;

    private bool playerDetected;

    private Vector3 ShootDirection => Vector3.right * (transform.localScale.x < 0f ? -1f : 1f);

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

                StartCoroutine(_ShootPlayer(hitPlayer.collider.gameObject));
            }
        }
    }

    IEnumerator _ShootPlayer(GameObject player)
    {
        GameObject ammo = Instantiate(ammoPrefab, transform.position + ShootDirection, transform.rotation);

        Vector3 direction = (player.transform.position - transform.position + ShootDirection);
        ammo.GetComponent<Rigidbody2D>().velocity = direction * ammoSpeed;

        yield return new WaitForSeconds(fireRate);

        playerDetected = false;
    }
}
