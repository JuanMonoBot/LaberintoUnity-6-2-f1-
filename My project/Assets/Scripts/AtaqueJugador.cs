using UnityEngine;

public class AtaqueJugador : MonoBehaviour
{
    public EnemyHealth enemy; // referencia al enemigo

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Tecla P detectada");

            if (enemy != null)
            {
                enemy.TakeDamage(20f);
            }
        }
    }
}