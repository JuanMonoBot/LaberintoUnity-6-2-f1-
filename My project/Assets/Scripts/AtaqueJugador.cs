using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;

    Camera cam;   // 👈 DECLARADA AQUÍ

    void Start()
    {
        cam = Camera.main;   // 👈 ASIGNADA AQUÍ
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Golpeó a: " + hit.transform.name);

            EnemyHealth enemy = hit.transform.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}