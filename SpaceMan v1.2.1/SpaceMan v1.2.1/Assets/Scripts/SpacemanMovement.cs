using UnityEngine;
using Cinemachine;

public class SpacemanMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public float gravityScale = 0.5f;
    public float maxBoostForce = 15f;
    public float boostFuel = 100f;
    public float boostRechargeRate = 10f;
    public float boostActivationThreshold = 20f;
    public float cameraRotationSpeed = 2f;
    public Transform cameraTarget;

    private Rigidbody2D rb;
    private bool isBoosting;
    private CinemachineFreeLook cinemachine;
    private bool facingLeft = false; // Corrected variable name

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cinemachine = Camera.main.GetComponent<CinemachineFreeLook>();

        // Check if the Rigidbody2D and CinemachineFreeLook components exist
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the hero GameObject.");
            enabled = false; // Disable the script to prevent further errors.
            return;
        }

        if (cinemachine == null)
        {
            Debug.LogError("CinemachineFreeLook component not found on the main camera GameObject.");
            enabled = false; // Disable the script to prevent further errors.
            return;
        }
    }

    private void Update()
    {
        // Null check for the Rigidbody2D component
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

        // Activate the jet boost if enough fuel is available
        if (Input.GetButton("Boost") && boostFuel >= boostActivationThreshold)
        {
            float boostForce = maxBoostForce;

            // Consume jet fuel while boosting
            boostFuel -= Time.deltaTime * boostRechargeRate;

            // Apply the boost force
            rb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse);
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }

        // Recharge jet fuel up to the maximum capacity
        if (!isBoosting && boostFuel < 100f)
        {
            boostFuel += Time.deltaTime * boostRechargeRate;
        }

        // Null check for the CinemachineFreeLook component
        if (cinemachine != null && cameraTarget != null)
        {
            cinemachine.Follow = cameraTarget.transform;

            Vector3 lookAtPosition = transform.position + transform.right;
            float angle = Mathf.Atan2(lookAtPosition.x - transform.position.x, lookAtPosition.z - transform.position.z) * Mathf.Rad2Deg;
            cinemachine.GetRig(0).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_FollowOffset.y = angle * cameraRotationSpeed;

            // Check for flipping based on movement direction
            if (horizontalInput < 0 && !facingLeft)
            {
                Flip();
            }
            else if (horizontalInput > 0 && facingLeft)
            {
                Flip();
            }
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
