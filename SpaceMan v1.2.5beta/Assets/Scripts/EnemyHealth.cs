using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; 
    private int currentHealth;
    public Slider healthSlider; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void takeDamage(int damageAmount)
    {
        if (damageAmount < 0)
        {
            currentHealth -= damageAmount;
            Debug.LogWarning("Negative damage received. Ensure damage is a positive value.");
            return;
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

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
}
