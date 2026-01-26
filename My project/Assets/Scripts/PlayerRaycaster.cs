using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRaycaster : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference shoot;  // Acción Shoot (Button)

    [Tooltip("Radio del raycast para interactuar con objetos")]
    public float rayRadius = 0.5f;

    [Header("Raycast")]
    public Camera mainCamera;                 // arrastra aquí tu Main Camera
    public float rayDistance = 30f;
    public LayerMask interactLayers = ~0;     // por defecto todo

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (shoot != null)
        {
            shoot.action.Enable();
            shoot.action.performed += OnShoot;
        }
    }

    private void OnDisable()
    {
        if (shoot != null)
        {
            shoot.action.performed -= OnShoot;
            shoot.action.Disable();
        }
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        TryShoot();
    }

    private void TryShoot()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        bool hasHit = false;

        // Si rayRadius > 0 usamos SphereCast, si no Raycast normal
        if (rayRadius > 0f)
        {
            hasHit = Physics.SphereCast(
                ray,
                rayRadius,
                out hit,
                rayDistance,
                interactLayers,
                QueryTriggerInteraction.Collide
            );
        }
        else
        {
            hasHit = Physics.Raycast(
                ray,
                out hit,
                rayDistance,
                interactLayers,
                QueryTriggerInteraction.Collide
            );
        }

        if (!hasHit) return;

        // Buscamos un DoorSwitch en el objeto golpeado o en sus padres
        DoorSwitch doorSwitch = hit.collider.GetComponentInParent<DoorSwitch>();
        if (doorSwitch != null)
        {
            doorSwitch.Activate();
            return;
        }

        // Aquí podrías añadir más tipos de interacción en el futuro.
    }
    /*
    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (mainCamera == null) return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactLayers, QueryTriggerInteraction.Collide))
        {
            // Debug para ver qué estamos golpeando
            Debug.Log("Raycast hit: " + hit.collider.name);
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);

            DoorSwitch doorSwitch = hit.collider.GetComponent<DoorSwitch>();
            if (doorSwitch != null)
            {
                doorSwitch.Activate();
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 1f);
        }
    }
    */
}
