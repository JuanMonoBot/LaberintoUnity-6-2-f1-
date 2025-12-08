using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MazePlayerController : MonoBehaviour
{
    [Header("Velocidad")]
    [Tooltip("Girar con A/D o flechas izq/der.")]
    public float turnSpeed = 180f;

    [Tooltip("Avanzar/retroceder con W/S o flechas arr/abajo.")]
    public float moveSpeed = 10f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 1) Leer input (Horizontal: A/D o flechas; Vertical: W/S o flechas)
        float turnInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");

        // 2) El personaje siempre avanza hacia donde mira
        float yaw = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);

        // 3) Calcular avance en espacio LOCAL (forward) y convertir a WORLD
        Vector3 localMove = new Vector3(0f, 0f, moveInput) * moveSpeed * Time.deltaTime;
        Vector3 worldMove = transform.TransformDirection(localMove);                     

       worldMove.y = verticalVelocity * Time.deltaTime;

        // 4) Mover usando CharacterController
        controller.Move(worldMove);
    }
}