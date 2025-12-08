using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target; // Player
    public Vector3 offset = new Vector3(0, 20, 0);

    [Header("Rotación fija (grados)")]
    public Vector3 fixedEuler = new Vector3(90f, 0f, 0f);

    void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(fixedEuler);
    }
}

