using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 10f;
    
    [Header("Gravedad / Salto")]
    public float gravity = -9.81f;
    public float jumpForce = 10f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;

    private CharacterController controller;
    private Vector2 rawMove = Vector2.zero;
    private float verticalVelocity;
    private bool mustJump = false;    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (move != null)
        {
            move.action.Enable();
            move.action.performed += __OnMove;
            move.action.canceled += __OnMove;
        }

        if (jump != null)
        {
            jump.action.Enable();
            jump.action.started += __OnJump;
        }
    }

    
    private void Update()
    {
        // Movimieto horizontal con Input Actions (x = izq/der, y = adelante/atrás)
        Vector3 movelocal = new Vector3(rawMove.x, 0f, rawMove.y) * moveSpeed;

        // Convertir  espacio mundo según hacia dónde mira el jugador
        Vector3 moveWorld = transform.TransformDirection(movelocal);

        // Gravedad y salto
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -1f; // Pequeña fuerza hacia abajo para mantener al jugador pegado al suelo
            }

            if (mustJump)
            {
                verticalVelocity = jumpForce;
                mustJump = false;
            }
        }
        verticalVelocity += gravity * Time.deltaTime;
        moveWorld.y = verticalVelocity;

        // Mover con CharacterController
        controller.Move(moveWorld * Time.deltaTime);
    }


    private void OnDisable()
    {
        if (move != null)
        {
            move.action.performed -= __OnMove;
            move.action.canceled -= __OnMove;
            move.action.Disable();
        }

        if (jump != null)
        {
            jump.action.started -= __OnJump;
            jump.action.Disable();
        }
    }


    private void __OnMove(InputAction.CallbackContext ctx)
    {
        rawMove = ctx.ReadValue<Vector2>();
        Debug.Log(ctx.control.device.name);
        Debug.Log("Input recibido: " + rawMove);
    }
    
    private void __OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            mustJump = true;
        Debug.Log(ctx.control.device.name);
    }  
     
}