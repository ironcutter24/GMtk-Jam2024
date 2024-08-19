using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] SpriteRenderer pressableSurface;
    [SerializeField] float startHeight = .375f;
    [SerializeField] float endHeight = .01f;
    [SerializeField] float speed = 5f; // Speed of the movement

    private bool isTriggered = false;
    private float t = 0f; // Interpolation parameter

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTriggered)
        {
            isTriggered = false;
        }
    }

    private void FixedUpdate()
    {
        // Update the interpolation parameter
        if (isTriggered)
        {
            t += speed * Time.fixedDeltaTime;
        }
        else
        {
            t -= speed * Time.fixedDeltaTime;
        }

        // Clamp t to ensure it stays between 0 and 1
        t = Mathf.Clamp01(t);

        // Interpolate the position using the updated t value
        pressableSurface.transform.localPosition = Vector3.Lerp(Vector3.up * startHeight, Vector3.up * endHeight, t);
    }
}
