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

    #region Stats Data

    private readonly Vector2 defaultSize = new Vector2(.8f, 1.8f);

    private readonly Dictionary<string, ParamValue[]> keyValuePairs = new Dictionary<string, ParamValue[]>()
    {
        {
            "Speed", new ParamValue[]
            {
                new ParamValue(5f, Vector2.zero),
                new ParamValue(10f, new Vector2(0f, 1f)),
                new ParamValue(15f, new Vector2(0f, 2f))
            }
        },
        {
            "Jump", new ParamValue[]
            {
                new ParamValue(5f, Vector2.zero),
                new ParamValue(9f, Vector2.zero),
                new ParamValue(14f, Vector2.zero)
            }
        },
        {
            "Weight", new ParamValue[]
            {
                new ParamValue(0f, Vector2.zero),
                new ParamValue(1f, Vector2.zero),
                new ParamValue(2f, new Vector2(.5f, 0f))
            }
        },
        {
            "Strength", new ParamValue[]
            {
                new ParamValue(0f, Vector2.zero),
                new ParamValue(1f, Vector2.zero),
                new ParamValue(2f, new Vector2(.5f, 1f))
            }
        },
    };

    private struct ParamValue
    {
        public float value;
        public Vector2 sizeDelta;

        public ParamValue(float value, Vector2 sizeDelta)
        {
            this.value = value;
            this.sizeDelta = sizeDelta;
        }
    }

    #endregion

    private float move;
    private bool isFlipped = false;
    private float gravityScale;
    private Flag jumpFlag = new Flag();

    private SpriteRenderer spriteRend;
    private Animator anim;
    private CapsuleCollider2D bodyCollider;
    private Rigidbody2D rb;

    [SerializeField] Transform graphics;
    [SerializeField, HideInInspector] Vector2 bounds;

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
    private Vector2 CeilingCircPos => rb.position + Vector2.up * (bounds.y - GroundCircRadius + CONTACT_CHECK_DEPTH * .5f);
    private Vector2 WallLeftBoxPos => rb.position + bounds.WithX(-bounds.x) * .5f;
    private Vector2 WallRightBoxPos => rb.position + bounds * .5f;

    private float GroundCircRadius => bounds.x * .46f;
    private Vector2 WallBoxSize => new Vector2(CONTACT_CHECK_DEPTH, bounds.y - bounds.x + bounds.x * .5f);

    private float MoveSpeed => keyValuePairs["Speed"][PlayerStats.Instance.MoveSpeed].value;
    private float JumpSpeed => keyValuePairs["Jump"][PlayerStats.Instance.JumpSpeed].value;

    private bool CanPlay
    {
        get
        {
            return GameManager.Instance.State == GameManager.GameState.Play
                || GameManager.Instance.State == GameManager.GameState.Tutorial
                || GameManager.Instance.State == GameManager.GameState.Menu;
        }
    }


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

        InputManager.Actions.Player.Move.performed += PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled += PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed += PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed += PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed += PlayerReloadLevel_performed;

        InputManager.SwitchActionMapToPlayer();
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsetPlayerController(this);

        PlayerStats.AnyChanged -= RefreshBounds;

        InputManager.Actions.Player.Move.performed -= PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled -= PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed -= PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed -= PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed -= PlayerReloadLevel_performed;
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

    public void RefreshBounds()
    {
        // Set bounds size

        var stats = PlayerStats.Instance;

        Vector2 totalSize = defaultSize;
        totalSize += keyValuePairs["Speed"][stats.MoveSpeed].sizeDelta;
        totalSize += keyValuePairs["Jump"][stats.JumpSpeed].sizeDelta;

        if (stats.Strength >= stats.Weight)
        {
            totalSize += keyValuePairs["Strength"][stats.Strength].sizeDelta;
        }
        else
        {
            totalSize += keyValuePairs["Weight"][stats.Weight].sizeDelta;
        }

        SetCharacterBounds(totalSize);


        // Set animation controller

        var controllers = GameManager.Instance.AnimControllers;
        int speedIndex = stats.MoveSpeed;

        RuntimeAnimatorController ctrl;
        if (stats.Strength >= 2)
        {
            ctrl = controllers.GetStrengthControllerAt(speedIndex);
        }
        else if (stats.Weight >= 2)
        {
            ctrl = controllers.GetWeightControllerAt(speedIndex);
        }
        else
        {
            ctrl = controllers.GetNormalControllerAt(speedIndex);
        }

        SetAnimationController(ctrl);
    }

    private void SetAnimationController(RuntimeAnimatorController ctrl)
    {
        anim.runtimeAnimatorController = ctrl;
    }

    private void SetCharacterBounds(Vector2 size)
    {
        bounds = size;
        bodyCollider.size = size;
        bodyCollider.offset = Vector2.up * size.y * .5f;
    }

    #region Physics checks

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
