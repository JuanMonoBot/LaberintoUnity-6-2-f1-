using UnityEngine;

public class PillarRailMovement : MonoBehaviour
{
    [Header("Pilar con cuchillas")]
    [Tooltip("Asignamos el objeto del pilar")]
    public Transform pilar; // Pilar que se moverá a lo largo del riel

    [Header("Recorrido del pilar")]
    [Tooltip("Asignamos los puntos entre los que se moverá el pilar")]
    public Transform startPoint; // Tope A del riel
    public Transform endPoint;   // Tope B del riel

    [Header("Velocidad de movimiento")]
    [Tooltip("Velocidad a la que se moverá el pilar a lo largo del riel")]
    public float movementSpeed = 2f; // Unidades por segundo

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool goingForward = true;

    private void Awake()
    {
        if (pilar == null)
            pilar = transform;

        // Guardamos las posiciones iniciales de los topes
        if (startPoint != null) startPosition = startPoint.position;
        else startPosition = pilar.position;

        if (endPoint != null) endPosition = endPoint.position;
        else endPosition = pilar.position;

        // El pilar arranca en el punto inicial
        pilar.position = startPosition;
    }

    private void Update()
    {
        if (pilar == null) return;
         // Determinamos la dirección del movimiento
         Vector3 target = goingForward ? endPosition : startPosition;

        // Movemos el pilar hacia el objetivo
        pilar.position = Vector3.MoveTowards(
            pilar.position,
            target,
            movementSpeed * Time.deltaTime
        );

        // Si hemos llegado al objetivo, cambiamos de dirección
        if (Vector3.Distance(pilar.position, target) < 0.01f)
        {
            goingForward = !goingForward;
        }
    }

    // Para resetar la trampa al reiniciar el nivel
    public void ResetTrap()
    {
        goingForward = true;
        pilar.position = startPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
            
        }
    }
}
