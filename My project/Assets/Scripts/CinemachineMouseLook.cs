using UnityEngine;
using Unity.Cinemachine; // namespace de Cinemachine
using UnityEngine.InputSystem;

public class CinemachineLookInputSystem : MonoBehaviour
{
    [Header("Referencias a Cinemachine")]
    public Transform playerBody;    // Referencia al cuerpo del jugador
    public CinemachineCamera cineCam;  // Referencia a la cámara Cinemachine

    [Header("Input Actions")]
    [SerializeField] private InputActionReference look; // Acción de mirar

    [Header("Sensibilidad del ratón")]
    public float sensitivity = 0.1f; // Sensibilidad del ratón
    public float minPitch = -80f;      // Ángulo mínimo de rotación de la camara
    public float maxPitch = 80f;       // Ángulo máximo de rotación de la camara

    private float pitch = 0f; // Rotación vertical acumulada
    private void OnEnable()
    {
        if (look != null)
            look.action.Enable();
    }

    private void OnDisable()
    {
        if (look != null)
            look.action.Disable();
    }

    private void LateUpdate()
    {
        if (look == null || cineCam == null || playerBody == null)
            return;

        // Leer delta del mouse / stick derecho
        Vector2 delta = look.action.ReadValue<Vector2>();
        //Debug.Log("Look delta: " + delta);

        float yawDelta = delta.x * sensitivity;
        float pitchDelta = delta.y * sensitivity;

        // 1) Girar el player en Y (izq/der)
        playerBody.Rotate(0f, yawDelta, 0f);

        // 2) Pitch de la cámara (arriba/abajo)
        pitch -= pitchDelta;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 3) Aplicar BOTH: pitch + yaw del player a la cámara
        Transform t = cineCam.transform;
        Vector3 euler = t.eulerAngles;
        euler.x = pitch;
        euler.y = playerBody.eulerAngles.y; // Igual que el player
        euler.z = 0f;
        t.eulerAngles = euler;
    }
}

