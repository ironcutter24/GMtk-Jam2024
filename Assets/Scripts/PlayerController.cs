using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class PlayerController : MonoBehaviour
{
    readonly Vector2 groundCheckBox = new Vector2(.94f, .06f);

    float move;
    float gravityScale;
    Flag jumpFlag = new Flag();
    Rigidbody2D rb;


    [Header("Jump")]
    [SerializeField, Min(0f)] float gravityScaleUp = 1f;
    [SerializeField, Min(0f)] float gravityScaleDown = 1f;

    [Header("Layers")]
    [SerializeField] LayerMask groundMask;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        gravityScale = gravityScaleDown;
    }

    private void Start()
    {
        InputManager.Actions.Player.Move.performed += PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled += PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed += PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed += PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed += PlayerReloadLevel_performed;

        InputManager.SwitchActionMapToPlayer();
    }

    private void OnDestroy()
    {
        InputManager.Actions.Player.Move.performed -= PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled -= PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed -= PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed -= PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed -= PlayerReloadLevel_performed;
    }

    private void FixedUpdate()
    {
        var velocity = rb.velocity;

        if (IsOnGround())
        {
            if (jumpFlag.Pop())
            {
                velocity.y = PlayerStats.Instance.JumpSpeed;
            }
            else
            {
                velocity.y = 0f;
            }
        }
        else if (IsOnCeiling() && velocity.y > 0f)
        {
            velocity.y = 0f;
        }
        else
        {
            gravityScale = velocity.y > 0 ? gravityScaleUp : gravityScaleDown;
            velocity.y -= 9.81f * gravityScale * Time.fixedDeltaTime;
        }

        velocity.x = move * PlayerStats.Instance.MoveSpeed;
        if (!Mathf.Approximately(move, 0f))
        {
            bool pushingLeft = move < 0f && IsOnWallLeft();
            bool pushingRight = move > 0f && IsOnWallRight();

            if (pushingLeft || pushingRight)
            {
                velocity.x = 0f;
            }
        }

        rb.velocity = velocity;
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

    private void PlayerReloadLevel_performed(InputAction.CallbackContext context)
    {
        GameManager.Instance.ReloadCurrentLevel();
    }

    private bool IsOnGround()
    {
        return Physics2D.OverlapBox(rb.position, groundCheckBox, 0f, groundMask);
    }

    private bool IsOnCeiling()
    {
        return Physics2D.OverlapBox(rb.position + Vector2.up, groundCheckBox, 0f, groundMask);
    }

    private bool IsOnWallRight()
    {
        return Physics2D.OverlapBox(rb.position + new Vector2(.5f, .5f), groundCheckBox, 90f, groundMask);
    }

    private bool IsOnWallLeft()
    {
        return Physics2D.OverlapBox(rb.position + new Vector2(-.5f, .5f), groundCheckBox, 90f, groundMask);
    }
}
