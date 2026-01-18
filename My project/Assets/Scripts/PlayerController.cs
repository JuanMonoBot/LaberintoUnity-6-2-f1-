using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Velocidad")]
    [Tooltip("Girar con A/D o flechas izq/der.")]
    public float turnSpeed = 180f;

    [Tooltip("Avanzar/retroceder con W/S o flechas arr/abajo.")]
    public float moveSpeed = 10f;

    [Header("Gravedad")]
    [Tooltip("Fuerza de gravedad aplicada al personaje.")]
    public float gravity = -9.81f;

    [Header("Salto")]
    [Tooltip("Fuerza aplicada al personaje al saltar.")]
    public float jumpForce = 10f;

    private CharacterController controller;
    private float verticalVelocity;

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference shoot;
    [SerializeField] InputActionReference jump;

    Vector2 rawMove = Vector2.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        shoot.action.Enable();
        jump.action.Enable();

        jump.action.started += __OnJump;

        move.action.started += __OnMove;
        move.action.performed += __OnMove;
        move.action.canceled += __OnMove;

        InputSystem.onAnyButtonPress.CallOnce(ctrl => Debug.Log($"Button {ctrl} was pessed on Device {ctrl.device}"));
    }

    //float verticalVelocity = 0f;
    private void Update()
    {
        // 1) Leer input (Horizontal: A/D o flechas; Vertical: W/S o flechas)
        float turnInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");

        // 2) El personaje siempre avanza hacia donde mira
        float yaw = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);

        // Salto
        if (mustJump)
        {
            verticalVelocity = jumpForce;
            mustJump = false;
        }

        // 3) Calcular avance en espacio LOCAL (forward) y convertir a WORLD
        Vector3 moveToApli = new Vector3(rawMove.x, 0f, rawMove.y) * moveSpeed * Time.deltaTime;
        transform.Translate(moveToApli, Space.Self);
        Vector3 localMove = new Vector3(0f, 0f, moveInput) * moveSpeed * Time.deltaTime;
        Vector3 worldMove = transform.TransformDirection(localMove);

        
        // 4) Gravedad simple para que el CharacterController se mantenga pegado al suelo
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -1f; // pequeño empuje hacia abajo para mantener grounded

        verticalVelocity += gravity * Time.deltaTime;
        worldMove.y = verticalVelocity * Time.deltaTime;
        

        // 5) Mover usando CharacterController
        controller.Move(worldMove);
    }


    private void OnDisable()
    {
        move.action.Disable();
        shoot.action.Disable();
        jump.action.Disable();

        jump.action.started -= __OnJump;

        move.action.started -= __OnMove;
        move.action.performed -= __OnMove;
        move.action.canceled -= __OnMove;
    }


    private void __OnMove(InputAction.CallbackContext ctx)
    {
        rawMove = ctx.ReadValue<Vector2>();
        Debug.Log(ctx.control.device.name);
        Debug.Log("Input recibido: " + rawMove);
    }

    bool mustJump = false;
    private void __OnJump(InputAction.CallbackContext ctx)
    {
        mustJump = ctx.ReadValueAsButton();
        Debug.Log(ctx.control.device.name);
    }


    
    /*
    void OnMove(InputValue value)
    {
        rawMove = value.Get<Vector2>();
        Debug.Log(rawMove);

    }

    void OnJump()
    {
         mustJump = true;
        Debug.Log("Salto");
    }
    */
    


}