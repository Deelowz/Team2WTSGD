using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class SpacemanMovement : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private bool _isMoving = false;
    private bool _isFacingRight = true;

    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    public bool IsFlying { get; private set; }
    public bool IsFacingRight => _isFacingRight;

    public float jumpForce = 2f;
    public float gravityScale = 0.5f;
    public float maxBoostForce = 5f;
    public float boostFuel = 100f;
    public float boostRechargeRate = 5f; // 5% per second
    public float boostActivationThreshold = 20f;
    public float cameraRotationSpeed = 2f;
    public Transform cameraTarget;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private bool isTouchingGround;

    private Rigidbody2D rb;
    private Animator animator;
    private CinemachineVirtualCamera cinemachine;

    private float boostDuration = 1f; // The duration of a single boost
    private float currentBoostTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cinemachine = Camera.main.GetComponent<CinemachineVirtualCamera>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the hero GameObject.");
            enabled = false;
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the hero GameObject.");
            enabled = false;
            return;
        }

        if (cinemachine == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found on the main camera GameObject.");
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component is not assigned.");
            return;
        }

        // Apply horizontal movement
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        // Apply jump force
        if (Mathf.Abs(rb.velocity.y) < 0.01f && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Activate the jet boost if enough fuel is available and within the boost duration
        if (Keyboard.current.spaceKey.isPressed && boostFuel >= boostActivationThreshold && currentBoostTime < boostDuration)
        {
            float boostForce = maxBoostForce;

            // Consume jet fuel while boosting
            boostFuel -= Time.deltaTime;

            // Apply the boost force
            rb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse);
            IsFlying = true;

            // Update the boost duration timer
            currentBoostTime += Time.deltaTime;
        }
        else
        {
            IsFlying = false;

            // Recharge jet fuel up to the maximum capacity at a rate of 5% per second
            if (boostFuel < 100f)
            {
                boostFuel += Time.deltaTime * (boostRechargeRate / 100f * 1f);
                boostFuel = Mathf.Clamp(boostFuel, 0f, 100f);
            }

            // Reset the boost duration timer
            currentBoostTime = 0f;
        }

        // Update the character's facing direction based on input
        SetFacingDirection(moveInput);
        animator.SetBool("isMoving", moveInput != Vector2.zero);
        animator.SetBool("isFlying", IsFlying);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x < 0)
        {
            _isFacingRight = false;
        }
        else if (moveInput.x > 0)
        {
            _isFacingRight = true;
        }

        Vector3 scale = transform.localScale;
        scale.x = _isFacingRight ? 1 : -1;
        transform.localScale = scale;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;
    }

    public void OnFly(InputAction.CallbackContext context)
    {
        IsFlying = context.performed;
    }
}
