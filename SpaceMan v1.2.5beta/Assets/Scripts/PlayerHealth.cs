using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);

        if (damageAmount <= 0)
        {
            Debug.LogWarning("Invalid or non-positive damage amount. Ensure damage is a positive value.");
            return;
        }

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (damageSound != null)
        {
            damageSound.Play();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player is defeated!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
