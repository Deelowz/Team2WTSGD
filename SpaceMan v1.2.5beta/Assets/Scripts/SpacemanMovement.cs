using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

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
    public Image boostFuelBar;
    public Slider boostFuelSlider;

    public int maxHealth = 100;
    public int health;
    private int currentHealth;
    public RectTransform healthBar;
    public Slider healthBarSlider;
    public float damageSoundVolume = 0.5f;
    public AudioClip damageSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    public Transform respawnPoint;

    private Rigidbody2D rb;
    private bool isBoosting;
    private CinemachineVirtualCamera cinemachine;
    private bool facingLeft = false;
    private float initialHealthBarWidth;
    private bool isDead = false;

    private void Start()
    {
        InitializeComponents();
        currentHealth = maxHealth;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        cinemachine = Camera.main.GetComponent<CinemachineVirtualCamera>();

        if (healthBar != null)
        {
            initialHealthBarWidth = healthBar.sizeDelta.x;
        }

        CheckComponents();
    }


    private void CheckComponents()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the hero GameObject.");
            DisableScript();
        }

        if (cinemachine == null)
        {
            Debug.LogError("CinemachineFreeLook component not found on the main camera GameObject.");
            DisableScript();
        }
    }

    private void DisableScript()
    {
        enabled = false; 
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleBoost();
        HandleCamera();
        HandleFlipping();
        UpdateHealthBar();
        UpdateBoostFuelBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void HandleMovement()
    {
        if (rb == null) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = moveDirection;
    }

    private void HandleJump()
    {
        if (rb == null) return;

        if (Mathf.Abs(rb.velocity.y) < 0.01f && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HandleBoost()
    {
        if (rb == null) return;

        if (Input.GetButton("Boost") && boostFuel >= boostActivationThreshold)
        {
            float boostForce = maxBoostForce;

            boostFuel -= Time.deltaTime * boostRechargeRate;

            rb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse);
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }

        if (!isBoosting && boostFuel < 100f)
        {
            boostFuel += Time.deltaTime * boostRechargeRate;
        }
    }

    private void HandleCamera()
    {
        if (cinemachine != null && cameraTarget != null)
        {
            cinemachine.m_Follow = transform;
            cinemachine.m_LookAt = transform;
        }
    }

    private void HandleFlipping()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if ((horizontalInput < 0 && !facingLeft) || (horizontalInput > 0 && facingLeft))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);

        // Play the damage sound when taking damage
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound, damageSoundVolume);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthBar.sizeDelta = new Vector2(initialHealthBarWidth * healthPercent, healthBar.sizeDelta.y);

            if (healthBarSlider != null)
            {
                Color healthColor = Color.Lerp(Color.red, Color.green, healthPercent);
                healthBarSlider.fillRect.GetComponent<Image>().color = healthColor;
            }
        }
    }

    private void UpdateBoostFuelBar()
    {
        if (boostFuelBar != null && boostFuelSlider != null)
        {
            float boostFuelPercentage = boostFuel / 100f; // Assuming boostFuel max is 100

            // Update the boost fuel bar's fill amount
            boostFuelBar.fillAmount = boostFuelPercentage;

            // Update the boost fuel slider's value
            boostFuelSlider.value = boostFuelPercentage;
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;

            // Play the death sound when the player dies
            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound, 1.0f);
            }

            Debug.Log("Player is defeated!");

            Invoke("Respawn", 0.6f);
        }
    }

    private void Respawn()
    {
        if (respawnPoint != null)
        {
            currentHealth = maxHealth;
            transform.position = respawnPoint.position;
            isDead = false;
        }
        else
        {
            Debug.LogError("Respawn Point is not assigned. Please assign a respawn point in the Inspector.");
        }
    }
}