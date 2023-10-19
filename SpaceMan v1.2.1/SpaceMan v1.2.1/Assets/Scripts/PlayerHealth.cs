using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform respawnPoint; // Assign the respawn point in the Inspector.

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implement player death logic, such as showing a game over screen.
        Debug.Log("Player is defeated!");

        // Respawn the player at the assigned respawn point.
        Respawn();
    }

    private void Respawn()
    {
        // Reset the player's health.
        currentHealth = maxHealth;

        // Move the player to the respawn point.
        transform.position = respawnPoint.position;
    }
}
