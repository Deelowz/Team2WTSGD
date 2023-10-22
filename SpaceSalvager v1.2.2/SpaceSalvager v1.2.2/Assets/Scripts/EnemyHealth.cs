using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Slider healthBar;
    public float maxHealth = 100f; 

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar();
    }

    private void Die()
    {
        // Play enemy death animation).
        Destroy(gameObject);
    }
}
