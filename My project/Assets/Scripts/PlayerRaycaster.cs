using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRaycaster : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference shoot;  // Acción Shoot (Button)

    [Header("Raycast")]
    public Camera mainCamera;                 // arrastra aquí tu Main Camera
    public float rayDistance = 30f;
    public LayerMask interactLayers = ~0;     // por defecto todo

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
}
