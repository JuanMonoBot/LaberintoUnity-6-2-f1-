using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 startPosition;
    private CharacterController controller;

    void Start()
    {
        startPosition = transform.position;
        controller = GetComponent<CharacterController>();
    }

    public void Respawn()
    {
        // Desactiva CharacterController para mover transform
        controller.enabled = false;

        // Mueve al jugador a la posición inicial
        transform.position = startPosition;

        // Reactiva CharacterController
        controller.enabled = true;

        Debug.Log("Jugador ha hecho respawn");
    }
}