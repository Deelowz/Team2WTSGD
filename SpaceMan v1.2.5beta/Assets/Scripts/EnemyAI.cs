using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    public float aggroRange = 10f;
    public float attackRange = 1.5f;
    public int damageAmount = 10;
    public int playerDamage = 10;

    private int maxHealth = 50;
    private int currentHealth;

    private bool isAttacking = false;
    private float attackInterval = 3.0f;
    private float attackTimer = 0.0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (player != null && currentHealth > 0)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < aggroRange)
            {
                Vector2 moveDirection = (player.position - transform.position).normalized;
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                if (distanceToPlayer < attackRange && !isAttacking)
                {
                    isAttacking = true;
                }
            }
            else
            {
                isAttacking = false;
            }

            if (isAttacking)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    AttackPlayer();
                    attackTimer = 0.0f;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Mathf.FloorToInt(playerDamage * Time.deltaTime));
            }
        }
    }

    private void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(playerDamage);
        }
    }

    public int GetDamageAmount()
    {
        return damageAmount;
    }

    private void TakeDamage()
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
