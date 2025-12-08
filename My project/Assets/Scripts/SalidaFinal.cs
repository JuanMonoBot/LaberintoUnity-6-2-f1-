using UnityEngine;

public class SalidaFinal : MonoBehaviour
{
    public ResetSalir ui; // Referencia al script ResetSalir
    public string playerTag = "Player"; // Tag del jugador
    public bool once = true; // Si es true, desactiva el trigger tras la primera activación

    // Detecta cuando el jugador entra en el trigger
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        ui.MostrarFelicitacion();

        if (once) GetComponent<Collider>().enabled = false;
    }
}