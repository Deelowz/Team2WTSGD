using UnityEngine;

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

                if (distanceToPlayer < attackRange)
                {
                    AttackPlayer();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
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
        return playerDamage;
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
