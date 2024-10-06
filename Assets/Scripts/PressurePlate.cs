using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private float t = 0f;

    [SerializeField] SpriteRenderer pressableSurface;
    [SerializeField] float startHeight = .375f;
    [SerializeField] float endHeight = .01f;
    [SerializeField] float speed = 5f;
    [SerializeField] bool stayPressed = false;
    [Space]
    [SerializeField] PressurePlate_Door targetDoor;

    public bool IsTriggered { get; private set; } = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanPressPlate(collision.gameObject))
        {
            if (!IsTriggered)
            {
                AudioManager.Instance.PlayPlatePress();
            }

            IsTriggered = true;
            targetDoor?.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CanPressPlate(collision.gameObject) && !stayPressed)
        {
            if (IsTriggered)
            {
                AudioManager.Instance.PlayPlatePress();
            }

            IsTriggered = false;
            targetDoor?.CloseDoor();
        }
    }

    private void FixedUpdate()
    {
        t += speed * Time.fixedDeltaTime * (!IsTriggered ? -1f : 1f);

        // Clamp t to ensure it stays between 0 and 1
        t = Mathf.Clamp01(t);

        // Interpolate the position using the updated t value
        pressableSurface.transform.localPosition = Vector3.Lerp(Vector3.up * startHeight, Vector3.up * endHeight, t);
    }

    private bool CanPressPlate(GameObject obj)
    {
        return obj.CompareTag("Player") || obj.CompareTag("Crate");
    }
}
