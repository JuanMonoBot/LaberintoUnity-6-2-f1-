using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Movimiento de apertura")]
    public float openHeight = 3f;   // cuánto sube
    public float openSpeed = 3f;    // unidades por segundo

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;

    private void Awake()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition;
    }

    private void Update()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                openSpeed * Time.deltaTime
            );
        }
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        targetPosition = closedPosition + Vector3.up * openHeight;
    }

    public void ResetDoor()
    {
        // Vuelve a la posición cerrada
        isOpen = false;
        targetPosition = closedPosition;
        transform.position = closedPosition; // Regresa inmediatamente a la posición cerrada
    }
}
