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

    bool playerDetected;

    private void FixedUpdate()
    {
        CheckPlayerInRange();
    }

    private void CheckPlayerInRange()
    {
        RaycastHit2D hitPlayer = Physics2D.CircleCast(transform.position, circleRadius, transform.right, maxDistance, layerMask);

        if(hitPlayer.collider != null)
        {
            if(playerDetected == false)
            {
                playerDetected = true;

                StartCoroutine(ShootPlayer(hitPlayer.collider.gameObject));
            }  
        }
    }

    IEnumerator ShootPlayer(GameObject player)
    {
        GameObject ammo = Instantiate(ammoPrefab, transform.position + new Vector3(1f, 0f, 0f), transform.rotation);

        Vector3 direction = (player.transform.position - transform.position + new Vector3(1f, 0f, 0f)).normalized;
        ammo.GetComponent<Rigidbody2D>().velocity = direction * ammoSpeed;

        yield return new WaitForSeconds(fireRate);

        playerDetected = false;
    }
}
