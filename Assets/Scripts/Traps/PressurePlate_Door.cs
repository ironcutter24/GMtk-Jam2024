using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_Door : MonoBehaviour
{ 
    Animator animator;
    Collider2D collider2D;

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
        collider2D = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        if(!doorIsOpen)
        {
            doorIsOpen = true;

            animator.Play("OpenDoor");
            collider2D.enabled = false;
        }
    }

    public void CloseDoor()
    {
        if(state == DoorState.keepPressed)
        {
            if (doorIsOpen)
            {
                doorIsOpen = false;

                animator.Play("CloseDoor");
                collider2D.enabled = true;
            }
        } 
    }
}
