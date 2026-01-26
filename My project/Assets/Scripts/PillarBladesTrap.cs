using UnityEngine;

public class PilarBladesTrap : MonoBehaviour
{
    [Header("Pilar con Cuchillas")]
    [Tooltip("Asignamos el objeto que contiene las cuchillas del pilar")]
    public Transform pillarBlades; // Pilar Giratorio

    [Header("Rotacion")]
    [Tooltip("Velocidad de rotación de las cuchillas del pilar")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 180f; // Grados por segundo
    public bool clockwise = true;

    private void Update()
    {
        if (pillarBlades == null) return;

        float dir = clockwise ? -1f : 1f;
        pillarBlades.Rotate(rotationAxis.normalized, rotationSpeed * dir * Time.deltaTime, Space.Self);
    }
}
