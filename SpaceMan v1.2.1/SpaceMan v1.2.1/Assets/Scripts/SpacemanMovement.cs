using UnityEngine;
using Cinemachine;

public class SpacemanMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public float gravityScale = 0.5f;
    public float maxBoostForce = 15f;
    public float boostFuel = 100f;
    public float boostRechargeRate = 5f; // 5% per second
    public float boostActivationThreshold = 20f;
    public float cameraRotationSpeed = 2f;
    public Transform cameraTarget;

    private Rigidbody2D rb;
    private bool isBoosting;
    private CinemachineFreeLook cinemachine;
    private bool facingLeft = false;

    private float boostDuration = 5f; // The duration of a single boost
    private float currentBoostTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cinemachine = Camera.main.GetComponent<CinemachineFreeLook>();

        // Check if the Rigidbody2D and CinemachineFreeLook components exist
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the hero GameObject.");
            enabled = false;
            return;
        }

        if (cinemachine == null)
        {
            Debug.LogError("CinemachineFreeLook component not found on the main camera GameObject.");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component is not assigned.");
            return;
        }

        // Move the character
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = moveDirection;

        // Apply jump force
        if (Mathf.Abs(rb.velocity.y) < 0.01f && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Activate the jet boost if enough fuel is available and within the boost duration
        if (Input.GetButton("Boost") && boostFuel >= boostActivationThreshold && currentBoostTime < boostDuration)
        {
            float boostForce = maxBoostForce;

            // Consume jet fuel while boosting
            boostFuel -= Time.deltaTime;

            // Apply the boost force
            rb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse);
            isBoosting = true;

            // Update the boost duration timer
            currentBoostTime += Time.deltaTime;
        }
        else
        {
            isBoosting = false;

            // Recharge jet fuel up to the maximum capacity at a rate of 5% per second
            if (boostFuel < 100f)
            {
                boostFuel += Time.deltaTime * (boostRechargeRate / 100f * 100f);
                boostFuel = Mathf.Clamp(boostFuel, 0f, 100f);
            }

            // Reset the boost duration timer
            currentBoostTime = 0f;
        }

        // Null check for the CinemachineFreeLook component
        if (cinemachine != null && cameraTarget != null)
        {
            // ...
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
