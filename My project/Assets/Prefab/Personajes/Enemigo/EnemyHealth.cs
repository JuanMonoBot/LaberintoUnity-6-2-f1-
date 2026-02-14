using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Image healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateBar();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}