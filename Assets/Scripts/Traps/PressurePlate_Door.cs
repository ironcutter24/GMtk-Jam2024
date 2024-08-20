using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_Door : MonoBehaviour
{
    Animator animator;
    Collider2D doorCollider2D;

    bool doorIsOpen;

    public DoorState state;

    public enum DoorState
    {
        oneClick,
        keepPressed
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        doorCollider2D = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        if (!doorIsOpen)
        {
            SetDoorOpen(true);
        }
    }

    public void CloseDoor()
    {
        if (state == DoorState.keepPressed)
        {
            if (doorIsOpen)
            {
                SetDoorOpen(false);
            }
        }
    }

    private void SetDoorOpen(bool isOpen)
    {
        doorIsOpen = isOpen;
        doorCollider2D.enabled = !isOpen;
        animator.Play(isOpen ? "OpenDoor" : "CloseDoor");
    }
}
