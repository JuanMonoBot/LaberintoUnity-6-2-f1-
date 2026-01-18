using UnityEngine;
using Unity.Cinemachine; // namespace de Cinemachine

public class CinemachineMouseLook : MonoBehaviour
{
    [Header("Referencias a Cinemachine")]
    public Transform playerBody;    // Referencia al cuerpo del jugador
    public CinemachineCamera cineCam;  // Referencia a la cámara Cinemachine

    [Header("Sensibilidad del ratón")]
    public float mouseSensitivity = 100f; // Sensibilidad del ratón
    public float minPitch = -45f;      // Ángulo mínimo de pitch
    public float maxPitch = 75f;       // Ángulo máximo de pitch

    private float pitch; // Rotación vertical acumulada

    private void Start()
    {
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        // Lee el mouse (Imput Manager clásico)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 1) Girar el cuerpo del jugador en el eje Y (izquierda/derecha)
        if (playerBody != null)
        {
            playerBody.Rotate(0f, mouseX* mouseSensitivity * Time.deltaTime, 0f);
        }

        // 2) Girar la cámara en el eje X (arriba/abajo)
        pitch -= mouseY * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 3) Aplicar la rotación a la cámara Cinemachine
        if (cineCam != null)
        {
            Transform t = cineCam.transform;

            Vector3 euler = t.localEulerAngles;
            euler.x = pitch;
            euler.z = 0f; // Sin inclinación lateral
        }
    }
}

