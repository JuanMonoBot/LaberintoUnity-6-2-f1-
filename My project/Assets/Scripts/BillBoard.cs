using UnityEngine;

public class BillboardFPS : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        Vector3 direction = cam.position - transform.position;
        direction.y = 0; // Evita inclinación vertical
        transform.rotation = Quaternion.LookRotation(-direction);
    }
}