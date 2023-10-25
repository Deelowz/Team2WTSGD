using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider; 
    public Transform respawnPoint;
    public AudioSource damageSound; 
    public AudioSource deathSound;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0)
    {
        Debug.LogWarning("Invalid or non-positive damage amount. Ensure damage is a positive value.");
        return;
    }
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth < 0)
        {
            Die();
        }
        else
        {
            if (damageSound != null)
            {
                damageSound.Play();
            }
        }
    }
    private void UpdateHealthUI()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            
        }
        else if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player is defeated!");
        
        if (deathSound != null)
        {
            deathSound.Play();
        }
        
        Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;

        transform.position = respawnPoint.position;
    }
}
