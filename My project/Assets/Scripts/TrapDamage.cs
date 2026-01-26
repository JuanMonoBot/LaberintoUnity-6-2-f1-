using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Tooltip("Cantidad de daño que inflige la trampa al jugador")]
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Identificamos el componente PlayerHealth del jugador
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Debug.Log("La trampa ha golpeado al jugador, vida restante: " + health.CurrentHearts);
        }
    }
}

