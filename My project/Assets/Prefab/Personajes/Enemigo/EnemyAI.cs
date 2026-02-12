using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrulla, // Estado de patrulla
        Sospecha, // Estado de sospecha
        Persecucion,  // Estado de persecución
        Investigacion, // Estado de investigación

    }

    [Header("Referencias")]
    public Transform[] waypoints; // Puntos de patrulla
    public Transform player; // Referencia al jugador
    public Animator anim; // Referencia al Animator del enemigo

    private NavMeshAgent agent; // Referencia al NavMeshAgent del enemigo

    [Header("Patrulla")]
    public float waitTime = 2f; // Tiempo de espera en cada punto de patrulla

    [Header("Vision")]
    public float viewRadius = 8f; // Radio de visión del enemigo
    [Range(0f, 360F)] public float viewAngle = 90f; // Ángulo de visión del enemigo
    public float eyeHeight = 1.7f; // Altura de los ojos del enemigo para la detección
    [Tooltip("Capas de obstáculos que bloquean la visión del enemigo.")]
    public LayerMask obstacleMask;

    [Header("Tiempos de estados")]
    [Tooltip("Tiempo que el enemigo debe ver al jugador para entrar en estado de persecución.")]
    public float timeToStartChase = 1.5f;

    [Tooltip("Tiempo que el enemigo se queda en estado de sospecha después de perder al jugador.")]
    public float searchDuration = 3f;

    private State currentState = State.Patrulla; // Estado inicial del enemigo
    private int currentWaypoint = 0; // Índice del punto de patrulla actual
    private float waitTimer = 0f; // Temporizador para el tiempo de espera en patrulla

    private float suspiciousTimer = 0f; // Temporizador para el estado de sospecha
    private float searchTimer = 0f; // Temporizador para el estado de investigación

    private Vector3 lastSeenPosition; // Última posición conocida del jugador

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            currentState = State.Patrulla;
            GoToNextWaypoint();
        }
        else
        {
            currentState = State.Patrulla;
            SetWalking(false); // Detener animación de caminar si no hay puntos de patrulla
            //Debug.LogWarning("No se han asignado puntos de patrulla para el enemigo.");
        }
    }

    private void GoToNextWaypoint()
    { 
    
    }


    // ------------------------
    //    Animaciones
    // ------------------------

    private void SetWalking(bool isWalking)
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", isWalking);
        }
    }
}

