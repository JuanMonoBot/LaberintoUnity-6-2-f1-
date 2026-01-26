using TMPro;
using UnityEngine;

public class UI_Menu : MonoBehaviour
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

    private Coroutine feedbackRoutine;

    [Header("Popup puertas cerradas")]
    public TextMeshProUGUI doorPopupText;
    public CanvasGroup doorPopupGroup;
    public float doorPopupFadeSpeed = 5f;

    private Coroutine doorPopupRoutine;

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

        // 4) Restablecer puertas e interruptores
        Door[] doors = Object.FindObjectsByType<Door>(FindObjectsSortMode.None);
        foreach (Door door in doors)
        {
            door.ResetDoor();
        }

        // 5) Resetear todos los interruptores
        DoorSwitch[] switches = Object.FindObjectsByType<DoorSwitch>(FindObjectsSortMode.None);
        foreach (DoorSwitch doorSwitch in switches)
        {
            doorSwitch.ResetSwitch();
        }

        // 6) Resetear vida del jugador
        PlayerHealth health = Object.FindFirstObjectByType<PlayerHealth>();
        if (health != null)
            health.ResetHealth();
    }


    public void ShowDoorPopup(string message)
    {
        if (doorPopupText == null || doorPopupGroup == null) return;

        doorPopupText.text = message;

        if (doorPopupRoutine != null)
            StopCoroutine(doorPopupRoutine);

        doorPopupRoutine = StartCoroutine(DoorPopupFade(1f));
    }

    public void HideDoorPopup()
    {
        if (doorPopupGroup == null) return;

        if (doorPopupRoutine != null)
            StopCoroutine(doorPopupRoutine);

        doorPopupRoutine = StartCoroutine(DoorPopupFade(0f));
    }

    private System.Collections.IEnumerator DoorPopupFade(float targetAlpha)
    {
        doorPopupGroup.gameObject.SetActive(true);

        while (!Mathf.Approximately(doorPopupGroup.alpha, targetAlpha))
        {
            doorPopupGroup.alpha = Mathf.MoveTowards(
                doorPopupGroup.alpha,
                targetAlpha,
                doorPopupFadeSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Si hemos llegado a 0, lo apagamos del todo
        if (Mathf.Approximately(targetAlpha, 0f))
        {
            doorPopupGroup.gameObject.SetActive(false);
        }
    }
       
}
