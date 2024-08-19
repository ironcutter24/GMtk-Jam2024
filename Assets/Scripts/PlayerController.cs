using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Util;

public class PlayerController : MonoBehaviour
{
    const float CONTACT_CHECK_DEPTH = .06f;
    const float CONTACT_CHECK_OFFSET = .06f;

    const string ANIM_GROUNDED_ID = "IsGrounded";
    const string ANIM_MOVING_ID = "IsMoving";
    const string ANIM_VERTICAL_SPEED_ID = "VerticalSpeed";

    private float move;
    private bool isFlipped = false;
    private float gravityScale;
    private Flag jumpFlag = new Flag();

    private Animator anim;
    private CapsuleCollider2D bodyCollider;
    private Rigidbody2D rb;

    [SerializeField] Transform graphics;
    [SerializeField] Vector2 bounds = Vector2.one;

    [Header("Jump")]
    [SerializeField, Min(0f)] float gravityScaleUp = 1f;
    [SerializeField, Min(0f)] float gravityScaleDown = 1f;

    [Header("Layers")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask ceilingMask;
    [SerializeField] LayerMask wallMask;

    private Vector2 GroundBoxSize => new Vector2(bounds.x - CONTACT_CHECK_OFFSET, CONTACT_CHECK_DEPTH);
    private Vector2 WallBoxSize => new Vector2(CONTACT_CHECK_DEPTH, bounds.y - CONTACT_CHECK_OFFSET);

    [Header("Body references:")]
    [SerializeField] GameObject playerChest;
    [SerializeField] GameObject playerLegs;

    [Header("Body parts (weight):")]
    [SerializeField] GameObject thinChest;
    [SerializeField] GameObject normalChest;
    [SerializeField] GameObject fatChest;

    [Header("Body parts (speed):")]
    [SerializeField] GameObject smallLegs;
    [SerializeField] GameObject normalLegs;
    [SerializeField] GameObject tallLegs;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        gravityScale = gravityScaleDown;
        SetCharacterBounds(bounds);

        GameManager.Instance.SetPlayerController(this);
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
        GameManager.Instance.UnsetPlayerController(this);

        InputManager.Actions.Player.Move.performed -= PlayerMove_performed;
        InputManager.Actions.Player.Move.canceled -= PlayerMove_performed;
        InputManager.Actions.Player.Jump.performed -= PlayerJump_performed;
        InputManager.Actions.Player.Interact.performed -= PlayerInteract_performed;
        InputManager.Actions.Player.ReloadLevel.performed -= PlayerReloadLevel_performed;
    }

    private void FixedUpdate()
    {
        var velocity = rb.velocity;

        var isGrounded = IsOnGround();
        if (isGrounded)
        {
            velocity.y = jumpFlag.Pop() ? PlayerStats.Instance.JumpSpeed : 0f;
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

        velocity.x = move * PlayerStats.Instance.MoveSpeed;

        var isMoving = !Mathf.Approximately(move, 0f);
        if (isMoving)
        {
            isFlipped = velocity.x < 0f;
            graphics.transform.localScale = isFlipped ? new Vector3(-1f, 1f, 1f) : Vector3.one;

            bool pushingLeft = move < 0f && IsOnWallLeft();
            bool pushingRight = move > 0f && IsOnWallRight();

            if (pushingLeft || pushingRight)
            {
                velocity.x = 0f;
            }
        }

        rb.velocity = velocity;

        // Animation parameters
        anim.SetBool(ANIM_MOVING_ID, isMoving);
        anim.SetBool(ANIM_GROUNDED_ID, isGrounded);
        anim.SetFloat(ANIM_VERTICAL_SPEED_ID, velocity.y);
    }

    public void Death()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
        PanelsManager.Instance.Open_Panel("GameOver_Panel");
    }

    private void SetCharacterBounds(Vector2 size)
    {
        bounds = size;
        bodyCollider.size = size;
        bodyCollider.offset = Vector2.up * size.y * .5f;
        //graphics.localScale = new Vector3(size.x, size.y, 1f);
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

    public void CheckAndSetSize(Slider sliderName)
    {
        if (sliderName.name == "SliderStrenght")
        {
            //Strength
            if (PlayerStats.Instance.Strength < 3)
            {
                SetCharacterBounds(new Vector2(0.5f, 0.5f));
            }
            else if (PlayerStats.Instance.Strength > 2 && PlayerStats.Instance.Strength < 4)
            {
                SetCharacterBounds(new Vector2(1f, 1f));
            }
            else if (PlayerStats.Instance.Strength > 3)
            {
                SetCharacterBounds(new Vector2(2f, 2f));
            }
        }
        else if (sliderName.name == "SliderWeight")
        {
            //Weight
            if (PlayerStats.Instance.Weight < 3)
            {
                SetCharacterBounds(new Vector2(0.5f, 1f));
            }
            else if (PlayerStats.Instance.Weight > 2 && PlayerStats.Instance.Weight < 4)
            {
                SetCharacterBounds(new Vector2(1f, 1f));
            }
            else if (PlayerStats.Instance.Weight > 3)
            {
                SetCharacterBounds(new Vector2(2f, 1f));
            }
        }
        else if (sliderName.name == "SliderSpeed")
        {
            //MoveSpeed
            if (PlayerStats.Instance.MoveSpeed < 3)
            {
                //Cambio la sprite delle gambe (Legs) del personaggio con quelle corte.
                //Adatto il collider alle nuove gambe.

                //SetCharacterBounds(new Vector2(1f, 0.5f));
            }
            else if (PlayerStats.Instance.MoveSpeed > 2 && PlayerStats.Instance.MoveSpeed < 4)
            {
                //SetCharacterBounds(new Vector2(1f, 1f));
                playerLegs = normalLegs;
            }
            else if (PlayerStats.Instance.MoveSpeed > 3)
            {
                //SetCharacterBounds(new Vector2(1f, 2f));
                playerLegs = tallLegs;
            }
        }
        else if (sliderName.name == "SliderJumpSpeed")
        {

        }
    }

    #region Physics checks

    private bool IsOnGround() => GroundCheck(rb.position, GroundBoxSize, groundMask);
    private bool IsOnCeiling() => GroundCheck(rb.position + new Vector2(0f, bounds.y), GroundBoxSize, ceilingMask);
    private bool IsOnWallLeft() => GroundCheck(rb.position + new Vector2(-bounds.x * .5f, bounds.y * .5f), WallBoxSize, wallMask);
    private bool IsOnWallRight() => GroundCheck(rb.position + new Vector2(bounds.x * .5f, bounds.y * .5f), WallBoxSize, wallMask);

    private bool GroundCheck(Vector2 pos, Vector2 size, LayerMask layerMask)
    {
        var hits = Physics2D.OverlapBoxAll(pos, size, 0f, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].isTrigger)
                return true;
        }
        return false;
    }

    #endregion

}
