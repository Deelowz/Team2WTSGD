using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damageAmount = 1;
    public Transform player;
    public float aggroRange = 10f;
    public float attackRange = 1.5f;
    public Transform respawnPoint; // Assign the respawn point in the Inspector

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Check if the player is within aggro range
            if (distanceToPlayer < aggroRange)
            {
                // Calculate the move direction towards the player
                Vector2 moveDirection = (player.position - transform.position).normalized;

                // Move towards the player
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                // Check if the player is within attack range
                if (distanceToPlayer < attackRange)
                {
                    // Attack the player
                    AttackPlayer();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Kill the player and reset their position
            KillPlayer(collision.transform);
        }
    }

    private void AttackPlayer()
    {
        // Implement your attack logic here
        // For example, you can play an attack animation or apply damage to the player
    }

    private void KillPlayer(Transform playerTransform)
    {
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }

        // Optionally perform any actions when the player dies (e.g., play death animation).
        // ...

        // Reset the player's position to the assigned respawn point.
        playerTransform.position = respawnPoint.position;
    }
}
