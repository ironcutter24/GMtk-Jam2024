using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class PlayerController : MonoBehaviour
{
    float move;
    Rigidbody2D rb;

    [SerializeField, Min(0f)] float moveSpeed = 2f;
    [SerializeField, Min(0f)] float jumpSpeed = 20f;
    [SerializeField, Min(0f)] float gravityScale = 1f;

    [SerializeField] LayerMask groundMask;

    Flag jumpFlag = new Flag();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InputManager.Actions.Player.Move.performed += PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled += PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed += PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed += PlayerInteract_performed;

        InputManager.SwitchActionMapToPlayer();
    }

    private void OnDestroy()
    {
        InputManager.Actions.Player.Move.performed -= PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled -= PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed -= PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed -= PlayerInteract_performed;
    }

    private void FixedUpdate()
    {
        var velocity = rb.velocity;

        if (IsGrounded())
        {
            velocity.y = jumpFlag.Pop() ? jumpSpeed : 0f;
        }
        else
        {
            velocity.y -= 9.81f * gravityScale * Time.fixedDeltaTime;
        }

        velocity.x = move * moveSpeed;

        rb.velocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(rb.position, new Vector2(.98f, .06f), 0f, groundMask);
    }

    private void PlayerMove_performed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<float>();
    }

    private void PlayerJump_performed(InputAction.CallbackContext context)
    {
        jumpFlag.Set();
    }

    private void PlayerInteract_performed(InputAction.CallbackContext context)
    {

    }
}
