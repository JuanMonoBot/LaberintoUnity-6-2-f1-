using UnityEngine;

public class SpinningTrap : MonoBehaviour
{
   [Header("Cuchillas giratorias")]
   [Tooltip("Asignamos el objeto que contiene las cuchillas giratorias")]
   public Transform bladesRotating;

    [Header("Rotación en intervalo")]
    [Tooltip("Eje de rotación de las cuchillas")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Grados por intervalo de rotación")]
    public float stepAngle = 90f;

    [Tooltip("Tiempo que tarda en girar la cuchilla")]
    public float rotateDuration = 0.3f;

    [Tooltip("Tiempo de espera entre giros")]
    public float pauseDuration = 2f;

    [Tooltip("Si es true, gira en sentido horario; si es false, en sentido antihorario")]
    public bool clockwise = true;

    //[Header("Daño al jugador")]
    [Tooltip("Cantidad de daño que inflige la trampa al jugador")]

    private void Start()
    {
        if (bladesRotating == null)
            bladesRotating = transform;

        // Iniciar la rutina de rotación
        rotationAxis = rotationAxis.normalized;
        StartCoroutine(RotateBladesLoop());
    }
     

    private System.Collections.IEnumerator RotateBladesLoop()
    {
        Transform t = bladesRotating;
        float dir = clockwise ? -1f : 1f;

        while (true)
        {
            // Rotacion inicial y final para este intervalo
            Quaternion startRotation = t.localRotation;
            Quaternion endRotation = startRotation * Quaternion.AngleAxis(stepAngle * dir, rotationAxis);

            float elapsed = 0f;

            // Giro suave durante rotateDuration
            while (elapsed < rotateDuration)
            {
                elapsed += Time.deltaTime;
                float lerp = Mathf.Clamp01(elapsed / rotateDuration);
                t.localRotation = Quaternion.Slerp(startRotation, endRotation, lerp);
                yield return null;
            }

            t.localRotation = endRotation;

            // Tiempo de espera entre giros
            if (pauseDuration > 0f)
                yield return new WaitForSeconds(pauseDuration);
            else
                yield return null;
        }
    }
}
