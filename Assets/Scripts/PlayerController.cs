using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class PlayerController : MonoBehaviour
{
    const float CONTACT_CHECK_DEPTH = .06f;
    const float CONTACT_CHECK_OFFSET = .12f;

    const string ANIM_GROUNDED_ID = "IsGrounded";
    const string ANIM_MOVING_ID = "IsMoving";
    const string ANIM_VERTICAL_SPEED_ID = "VerticalSpeed";

    private float move;
    private bool isFlipped = false;
    private float gravityScale;
    private Flag jumpFlag = new Flag();

    private SpriteRenderer spriteRend;
    private Animator anim;
    private CapsuleCollider2D bodyCollider;
    private Rigidbody2D rb;

    [SerializeField] StatsSheet_SO statsSheet;
    [SerializeField] Transform graphics;
    public Vector2 Bounds { get; private set; }

    [Header("Jump")]
    [SerializeField, Min(0f)] float gravityScaleUp = 1f;
    [SerializeField, Min(0f)] float gravityScaleDown = 1f;

    [Header("Layers")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask ceilingMask;
    [SerializeField] LayerMask wallMask;
    [Space]
    [SerializeField] LayerMask pushableMask;


    private Vector2 GroundCircPos => rb.position + Vector2.up * (GroundCircRadius - CONTACT_CHECK_DEPTH * .5f);
    private Vector2 CeilingCircPos => rb.position + Vector2.up * (Bounds.y - GroundCircRadius + CONTACT_CHECK_DEPTH * .5f);
    private Vector2 WallLeftBoxPos => rb.position + Bounds.WithX(-Bounds.x) * .5f;
    private Vector2 WallRightBoxPos => rb.position + Bounds * .5f;

    private float GroundCircRadius => Bounds.x * .46f;
    private Vector2 WallBoxSize => new Vector2(CONTACT_CHECK_DEPTH, Bounds.y - Bounds.x + Bounds.x * .5f);

    private float MoveSpeed => statsSheet.Speed.GetParamAt(PlayerStats.Instance.MoveSpeed).value;
    private float JumpSpeed => statsSheet.Jump.GetParamAt(PlayerStats.Instance.JumpSpeed).value;

    private bool CanPlay => GameManager.Instance.State switch
    {
        GameManager.GameState.Tutorial => true,
        GameManager.GameState.Play => true,
        GameManager.GameState.Menu => true,
        _ => false
    };


    private void Awake()
    {
        spriteRend = graphics.GetComponent<SpriteRenderer>();
        anim = graphics.GetComponent<Animator>();

        bodyCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        GameManager.Instance.SetPlayerController(this);
    }

    private void Start()
    {
        gravityScale = gravityScaleDown;
        RefreshBounds();

        PlayerStats.AnyChanged += RefreshBounds;

        RegisterInputs();

        InputManager.SwitchActionMapToPlayer();
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsetPlayerController(this);

        PlayerStats.AnyChanged -= RefreshBounds;

        UnregisterInputs();
    }

    private void FixedUpdate()
    {
        if (!CanPlay) return;

        var velocity = rb.velocity;

        var isGrounded = IsOnGround();
        if (isGrounded)
        {
            if (jumpFlag.Pop())
            {
                velocity.y = JumpSpeed;
                AudioManager.Instance.PlayPlayerJump();
            }
            else
            {
                velocity.y = 0f;
            }
        }
        else
        {
            if (IsOnCeiling() && velocity.y > 0f)
            {
                velocity.y = 0f;
            }
            else
            {
                gravityScale = velocity.y > 0 ? gravityScaleUp : gravityScaleDown;
                velocity.y -= 9.81f * gravityScale * Time.fixedDeltaTime;
            }
        }

        velocity.x = move * MoveSpeed;

        var isMoving = !Mathf.Approximately(move, 0f);
        if (isMoving)
        {
            isFlipped = velocity.x < 0f;
            spriteRend.flipX = isFlipped;

            bool pushingLeft = move < 0f && IsOnWallLeft();
            bool pushingRight = move > 0f && IsOnWallRight();

            if (pushingLeft || pushingRight)
            {
                TryMovePushable(-velocity.x);
                velocity.x = 0f;
                isMoving = false;
            }
        }

        rb.velocity = velocity;

        // Animation parameters
        anim.SetBool(ANIM_MOVING_ID, isMoving);
        anim.SetBool(ANIM_GROUNDED_ID, isGrounded);
        anim.SetFloat(ANIM_VERTICAL_SPEED_ID, velocity.y);
    }

    private void TryMovePushable(float horizontalSpeed)
    {
        var pushable = IsOnPushable()?.gameObject.GetComponentInParent<Rigidbody2D>();
        if (pushable)
        {
            pushable.velocity = new Vector2(horizontalSpeed, pushable.velocity.y);
        }
    }

    #region Input Registration

    private void RegisterInputs()
    {
        InputManager.Actions.Player.Move.performed += PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled += PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed += PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed += PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed += PlayerReloadLevel_performed;
    }

    private void UnregisterInputs()
    {
        InputManager.Actions.Player.Move.performed -= PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled -= PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed -= PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed -= PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed -= PlayerReloadLevel_performed;
    }

    #endregion

    #region Input Implementation

    private void PlayerMove_performed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<float>();
    }

    private void PlayerJump_performed(InputAction.CallbackContext context)
    {
        if (IsOnGround())
        {
            jumpFlag.Set();
        }
    }

    private void PlayerInteract_performed(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Interaction was not implemented.");
    }

    private void PlayerReloadLevel_performed(InputAction.CallbackContext context)
    {
        GameManager.Instance.ReloadCurrentLevel();
    }

    #endregion

    #region Physics Bounds

    private void RefreshBounds()
    {
        // Set bounds size
        var size = GetCharacterSize();
        SetCharacterBounds(size);

        // Set animation controller
        var ctrl = GetRequiredAnimController();
        SetAnimController(ctrl);
    }

    private Vector2 GetCharacterSize()
    {
        var stats = PlayerStats.Instance;

        Vector2 totalSize = statsSheet.DefaultSize;
        totalSize += statsSheet.Speed.GetParamAt(stats.MoveSpeed).sizeDelta;
        totalSize += statsSheet.Jump.GetParamAt(stats.JumpSpeed).sizeDelta;

        if (stats.Strength >= stats.Weight)
        {
            totalSize += statsSheet.Strength.GetParamAt(stats.Strength).sizeDelta;
        }
        else
        {
            totalSize += statsSheet.Weight.GetParamAt(stats.Weight).sizeDelta;
        }

        return totalSize;
    }

    private RuntimeAnimatorController GetRequiredAnimController()
    {
        var controllers = GameManager.Instance.AnimControllers;

        var stats = PlayerStats.Instance;
        int speedIndex = stats.MoveSpeed;

        if (stats.Strength >= 2)
        {
            return controllers.GetStrengthControllerAt(speedIndex);
        }
        else if (stats.Weight >= 2)
        {
            return controllers.GetWeightControllerAt(speedIndex);
        }
        else
        {
            return controllers.GetNormalControllerAt(speedIndex);
        }
    }

    private void SetAnimController(RuntimeAnimatorController ctrl)
    {
        anim.runtimeAnimatorController = ctrl;
    }

    private void SetCharacterBounds(Vector2 size)
    {
        Bounds = size;
        bodyCollider.size = size;
        bodyCollider.offset = Vector2.up * size.y * .5f;
    }

    #endregion

    #region Physics Checks

    private Collider2D IsOnPushable() => GroundCheck(GroundCircPos, GroundCircRadius, pushableMask);
    private Collider2D IsOnGround() => GroundCheck(GroundCircPos, GroundCircRadius, groundMask);
    private Collider2D IsOnCeiling() => GroundCheck(CeilingCircPos, GroundCircRadius, ceilingMask);
    private Collider2D IsOnWallLeft() => GroundCheck(WallLeftBoxPos, WallBoxSize, wallMask);
    private Collider2D IsOnWallRight() => GroundCheck(WallRightBoxPos, WallBoxSize, wallMask);


    private Collider2D GroundCheck(Vector2 pos, Vector2 size, LayerMask layerMask)
    {
        var hits = Physics2D.OverlapBoxAll(pos, size, 0f, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].isTrigger)
                return hits[i];
        }
        return null;
    }

    private Collider2D GroundCheck(Vector2 pos, float radius, LayerMask layerMask)
    {
        var hits = Physics2D.OverlapCircleAll(pos, radius, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].isTrigger)
                return hits[i];
        }
        return null;
    }

    #endregion

    #region Death

    public void Death(DeathType deathType)
    {
        PlayDeathAudio(deathType);
        GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
    }

    private void PlayDeathAudio(DeathType deathType)
    {
        switch (deathType)
        {
            case DeathType.Blade:
                AudioManager.Instance.PlayPlayerDeathBlade();
                break;

            case DeathType.Drown:
                AudioManager.Instance.PlayPlayerDeathDrown();
                break;

            case DeathType.Knight:
                AudioManager.Instance.PlayPlayerDeathKnight();
                break;

            default:
                AudioManager.Instance.PlayGameOver();
                break;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying && rb == null)
            rb = GetComponent<Rigidbody2D>();

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(GroundCircPos, GroundCircRadius);
        Gizmos.DrawWireSphere(CeilingCircPos, GroundCircRadius);
        Gizmos.DrawWireCube(WallLeftBoxPos, WallBoxSize);
        Gizmos.DrawWireCube(WallRightBoxPos, WallBoxSize);
    }

}
