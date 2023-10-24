using UnityEngine;
using UnityEngine.UI;

public class GroundEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public int maxHealth = 50;
    private int currentHealth;

    public Slider healthSlider;
    public int damageAmount = 10;

    private Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public AudioSource shootingSound;

    private bool isDead = false;
    private float nextFireTime;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (isDead)
            return;

        if (player == null)
        {
            FindPlayer();
            return;
        }

        MoveTowardsPlayer();

        if (IsPlayerInRange())
        {
            TryShoot();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }

    private void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 shootingDirection = (player.position - firePoint.position).normalized;
            rb.velocity = shootingDirection * moveSpeed;
        }

        if (shootingSound != null)
        {
            shootingSound.Play();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Perform death animations or actions here

        DisableComponents();
        Destroy(gameObject, 2.0f);
    }

    private void DisableComponents()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
}
