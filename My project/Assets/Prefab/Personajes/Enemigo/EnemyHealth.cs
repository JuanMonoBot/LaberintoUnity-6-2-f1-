using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    float currentHealth;

    public Image healthFill;

    // 🔹 Se ejecuta antes que Start
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        UpdateBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida actual: " + currentHealth); // Para comprobar

        UpdateBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateBar()
    {
        healthFill.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log("Enemigo eliminado");
        Destroy(gameObject);
    }
}