using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class PlayerController : MonoBehaviour
{
    const float contactCheckDepth = .06f;
    const float contactCheckOffset = .06f;

    private Vector2 bounds = Vector2.one;

    private float move;
    private float gravityScale;
    private Flag jumpFlag = new Flag();
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    [SerializeField] Transform graphics;

    [Header("Jump")]
    [SerializeField, Min(0f)] float gravityScaleUp = 1f;
    [SerializeField, Min(0f)] float gravityScaleDown = 1f;

    [Header("Layers")]
    [SerializeField] LayerMask groundMask;

    private Vector2 GroundBoxSize => new Vector2(bounds.x - contactCheckOffset, contactCheckDepth);
    private Vector2 WallBoxSize => new Vector2(bounds.y - contactCheckOffset, contactCheckDepth);


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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

        //SetCharacterBounds(new Vector2(2, 2));
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
            velocity.y = jumpFlag.Pop() ? PlayerStats.Instance.JumpSpeed : 0f;
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

    private void SetCharacterBounds(Vector2 size)
    {
        bounds = size;
        boxCollider.size = size;
        boxCollider.offset = Vector2.up * size.y * .5f;
        graphics.localScale = new Vector3(size.x, size.y, 1f);
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
        Debug.LogWarning("Interaction was not implemented.");
    }

    private void PlayerReloadLevel_performed(InputAction.CallbackContext context)
    {
        GameManager.Instance.ReloadCurrentLevel();
    }

    #region Physics checks

    private bool IsOnGround() => Physics2D.OverlapBox(rb.position, GroundBoxSize, 0f, groundMask);
    private bool IsOnCeiling() => Physics2D.OverlapBox(rb.position + new Vector2(0f, bounds.y), GroundBoxSize, 0f, groundMask);
    private bool IsOnWallLeft() => Physics2D.OverlapBox(rb.position + new Vector2(-bounds.x * .5f, bounds.y * .5f), WallBoxSize, 90f, groundMask);
    private bool IsOnWallRight() => Physics2D.OverlapBox(rb.position + new Vector2(bounds.x * .5f, bounds.y * .5f), WallBoxSize, 90f, groundMask);

    #endregion

}
