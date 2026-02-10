using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    [Tooltip("Puntos de patrulla para el enemigo.")]
    public Transform[] waypoints;

    [Tooltip("Tiempo de espera en cada punto de patrulla.")]
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private Animator anim;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            currentWaypointIndex = 0;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            SetWalking(true);
        }
        else
        {
            SetWalking(false); // Detener animación de caminar si no hay puntos de patrulla
            //Debug.LogWarning("No se han asignado puntos de patrulla para el enemigo.");
        }
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextPoint();
            }
            return;
        }

        // Verificar si el agente ha llegado al destino
        if (agent.pathPending)
            return;

        // Si el agente ha llegado al destino, iniciar el tiempo de espera
        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            // Iniciar tiempo de espera
            isWaiting = true;
            waitTimer = waitTime;
            agent.isStopped = true;
            SetWalking(false);
        }
        else
        {
            // Continuar caminando
            agent.isStopped = false;
            SetWalking(true);
        }
    }

    private void GoToNextPoint()
    {
        if (waypoints.Length == 0)
            return;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Avanzar al siguiente punto de patrulla
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        SetWalking(true);
    }

    private void SetWalking(bool isWalking)
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", isWalking);
            Debug.Log("isWalking =" + isWalking);
        }
    }
}
