using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private Vector3 startPosition;
    private CharacterController controller;

    void Start()
    {
        startPosition = transform.position;
        controller = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Si el objeto con el que chocamos tiene tag Enemy
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        controller.enabled = false;
        transform.position = startPosition;
        controller.enabled = true;

        Debug.Log("Jugador ha hecho respawn por colisión");
    }
}