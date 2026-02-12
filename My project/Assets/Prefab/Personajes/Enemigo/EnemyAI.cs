using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrulla,      // Estado de patrulla
        Sospecha,      // Estado de sospecha
        Persecucion,   // Estado de persecución
        Investigacion, // Estado de investigación
    }

    [Header("Referencias")]
    public Transform[] waypoints; // Puntos de patrulla
    public Transform player;      // Referencia al jugador
    public Animator anim;         // Referencia al Animator del enemigo

    private NavMeshAgent agent;   // Referencia al NavMeshAgent del enemigo

    [Header("Patrulla")]
    public float waitAtPoint = 2f; // Tiempo de espera en cada punto de patrulla

    [Header("Visión")]
    public float viewRadius = 8f;          // Radio de visión del enemigo
    [Range(0f, 360f)] public float viewAngle = 90f; // Ángulo de visión del enemigo
    public float eyeHeight = 1.7f;         // Altura de los ojos del enemigo para la detección

    [Tooltip("Capas de obstáculos que bloquean la visión del enemigo.")]
    public LayerMask obstacleMask;

    [Tooltip("Capa donde está el Player.")]
    public LayerMask playerMask;

    [Header("Tiempos de estados")]
    [Tooltip("Tiempo que el enemigo debe ver al jugador para entrar en estado de persecución.")]
    public float timeToStartChase = 1.5f;

    [Tooltip("Tiempo que el enemigo se queda en estado de investigación después de perder al jugador.")]
    public float searchDuration = 3f;

    [Header("Debug")]
    public bool debugVision = true; // DEBUG: dibujar rayos y mostrar logs

    private State currentState = State.Patrulla; // Estado inicial del enemigo
    private int currentWaypoint = 0;             // Índice del punto de patrulla actual
    private float waitTimer = 0f;               // Temporizador para el tiempo de espera en patrulla

    private float suspiciousTimer = 0f; // Temporizador para el estado de sospecha
    private float searchTimer = 0f;     // Temporizador para el estado de investigación

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
          //Debug.LogWarning("EnemyAI: No se han asignado puntos de patrulla para el enemigo.");
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrulla:
                PatrolUpdate();
                break;
            case State.Sospecha:
                SuspiciousUpdate();
                break;
            case State.Persecucion:
                ChaseUpdate();
                break;
            case State.Investigacion:
                SearchUpdate();
                break;
        }
    }

    // ------------------------
    //    Actualizaciones de estados
    // ------------------------

    #region Patrulla

    private void PatrolUpdate()
    {
        // DEBUG: ver en consola el estado actual
        // Debug.Log("Estado actual: " + currentState);

        // Verificar si el enemigo ve al jugador mientras patrulla
        if (CanSeePlayer())
        {
            lastSeenPosition = player.position;
            if (debugVision) Debug.Log("EnemyAI: ¡He visto al jugador! Paso a estado Sospecha.");
            EnterSospecha();
            return;
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            SetWalking(false); // Detener animación de caminar si no hay puntos de patrulla
            return;
        }

        // Importante: esperar a que el agente tenga ruta
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            // El enemigo ha llegado al punto de patrulla, esperar un momento antes de ir al siguiente
            if (waitTimer <= 0f)
            {
                waitTimer = waitAtPoint;
                SetWalking(false);
                agent.isStopped = true;
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    GoToNextWaypoint();
                }
            }
        }
        else
        {
            agent.isStopped = false;
            SetWalking(true);
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypoint].position);
        SetWalking(true);
    }

    #endregion

    #region Sospecha

    private void EnterSospecha()
    {
        currentState = State.Sospecha;
        suspiciousTimer = timeToStartChase;
        SetWalking(false);
        agent.isStopped = true;
    }

    private void SuspiciousUpdate()
    {
        bool canSee = CanSeePlayer();

        if (canSee)
        {
            lastSeenPosition = player.position;
            suspiciousTimer -= Time.deltaTime;

            // Si el enemigo ha visto al jugador durante suficiente tiempo, entrar en persecución
            if (suspiciousTimer <= 0f)
            {
                if (debugVision) Debug.Log("EnemyAI: Tiempo de sospecha completado, entrando en Persecución.");
                EnterPersecucion();
            }
        }
        else
        {
            // Si el enemigo pierde de vista al jugador, entrar en estado de investigación
            if (debugVision) Debug.Log("EnemyAI: Perdí de vista al jugador durante la sospecha, entrando en Investigación.");
            EnterInvestigacion();
        }
    }

    #endregion

    #region Persecución

    private void EnterPersecucion()
    {
        currentState = State.Persecucion;
        agent.isStopped = false;
        SetWalking(true);
    }

    private void ChaseUpdate()
    {
        if (player == null)
        {
            // Si por algún motivo el jugador desaparece, volvemos a patrulla
            if (debugVision) Debug.LogWarning("EnemyAI: player == null en Persecución, vuelvo a Patrulla.");
            ReturnToPatrol();
            return;
        }

        bool canSee = CanSeePlayer();

        if (canSee)
        {
            lastSeenPosition = player.position;
            agent.SetDestination(player.position);
        }
        else
        {
            // Si el enemigo pierde de vista al jugador, entrar en estado de investigación
            if (debugVision) Debug.Log("EnemyAI: Perdí de vista al jugador en Persecución, entrando en Investigación.");
            EnterInvestigacion();
        }
    }

    #endregion

    #region Investigacion

    private void EnterInvestigacion()
    {
        currentState = State.Investigacion;
        searchTimer = searchDuration;
        agent.isStopped = false;
        agent.SetDestination(lastSeenPosition);
        SetWalking(true);
    }

    private void SearchUpdate()
    {
        // Si el enemigo ve al jugador durante la investigación, volver a entrar en persecución
        if (CanSeePlayer())
        {
            lastSeenPosition = player.position;
            if (debugVision) Debug.Log("EnemyAI: He vuelto a ver al jugador durante la investigación, regreso a Persecución.");
            EnterPersecucion();
            return;
        }

        if (agent.pathPending)
            return;  // Esperar a que el agente calcule la ruta antes de verificar la distancia restante

        // Si el enemigo llega a la última posición conocida del jugador, esperar un momento antes de volver a patrullar
        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            // El enemigo ha llegado a la última posición conocida del jugador, esperar un momento antes de volver a patrullar
            agent.isStopped = true;
            SetWalking(false);

            // El enemigo gira para mirar alrededor durante la investigación
            transform.Rotate(0f, 60f * Time.deltaTime, 0f);

            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f)
            {
                // El enemigo no encontró al jugador durante la investigación, volver a patrullar
                ReturnToPatrol();
            }
        }
        else
        {
            agent.isStopped = false;
            SetWalking(true);
        }
    }

    private void ReturnToPatrol()
    {
        currentState = State.Patrulla;
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypoint].position);
        SetWalking(true);
    }

    #endregion

    // ------------------------
    //    Lógica de visión
    // ------------------------

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;

        bool playerEnRadio = Physics.CheckSphere(
        origin,                  // centro de la esfera
        viewRadius,              // radio
        playerMask,              // solo miramos la capa Player
        QueryTriggerInteraction.Collide
    );
        if (!playerEnRadio)
            return false; // El jugador está demasiado lejos

        Vector3 target = player.position + Vector3.up * 1.0f; // Ajustar para la altura del jugador
        Vector3 dir = target - origin;
        float distance = dir.magnitude;

        dir /= distance; // normalizar

        // DEBUG: dibujar línea hacia el jugador
        if (debugVision)
        {
            Debug.DrawLine(origin, target, Color.yellow);
        }

        // Comprobar distancia
        if (distance > viewRadius)
            return false;

        dir.Normalize();

        //  Comprobar cono de visión.
        //    Si quieres "visión 360°", pon viewAngle = 360 y esto casi nunca fallará.
        if (viewAngle < 360f)
        {
            float angle = Vector3.Angle(transform.forward, dir);
            if (angle > viewAngle * 0.5f)
                return false; // El jugador está fuera del cono de visión
        }

        // Comprobar línea de visión con un raycast.
        //    Máscara = jugador + obstáculos (paredes).
        int mask = playerMask | obstacleMask;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, distance, mask, QueryTriggerInteraction.Collide))
        {
            if (debugVision)
            {
                // Verde si el primer impacto es el Player, rojo si es una pared
                Color c = hit.collider.CompareTag("Player") ? Color.green : Color.red;
                Debug.DrawLine(origin, hit.point, c);
            }

            // Si lo primero que golpea es el Player -> lo vemos.
            return hit.collider.CompareTag("Player");
        }
        // Si el raycast no golpea nada, no vemos al jugador
        return false;
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
