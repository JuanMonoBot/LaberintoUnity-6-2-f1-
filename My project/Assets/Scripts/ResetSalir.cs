using UnityEngine;

public class ResetSalir : MonoBehaviour
{
    [Header("Winer")]
    public GameObject winPanel; // Colocamos el panel del Canvas que tiene las felicitaciones

    public void MostrarFelicitacion()
    {
        if (winPanel) winPanel.SetActive(true);
    }   

    [Header("Referencias")]
    public Transform player;   // Colocamos el objeto Player     
    public Transform startPoint;   // Colocamos el punto de inicio   

    public Collider exitTriggerCollider;

    public void SalirDelJuego()
    {
        Application.Quit();

        // Para que "Salir" funcione en el Editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ReiniciarPlayer()
    {
        // 1) Ocultar felicitaciones
        if (winPanel) winPanel.SetActive(false);

        // 2) Reactivar el trigger de salida si está desactivado
        if (exitTriggerCollider) exitTriggerCollider.enabled = true;

        // 3) Teletransportar al player al punto de inicio
        if (!player || !startPoint) return;

        // Desactivamos el CharacterController para evitar problemas al teletransportar
        var cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.SetPositionAndRotation(startPoint.position, startPoint.rotation);

        // Reactivamos el CharacterController
        if (cc) cc.enabled = true;
    }
}
