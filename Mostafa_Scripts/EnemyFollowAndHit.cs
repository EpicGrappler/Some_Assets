using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Unity.XR.CoreUtils;

public class EnemyFollowAndHit : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;                 // LEAVE EMPTY IN PREFAB
    public float followDistance = 1.2f;
    public float moveSpeed = 2.5f;

    [Header("Hit System")]
    public int maxHits = 3;
    public float hitCooldown = 1f;
    public DropAndRespawn respawnSystem;     // OPTIONAL (scene object)
    public HitCounterUI hitUI;               // OPTIONAL (scene object)

    private int currentHits = 0;
    private float lastHitTime;

    private NavMeshAgent agent;
    private Animator animator;
    private XRGrabInteractable grab;
void OnEnable()
{
    ResolveDependencies();
}
    void Start()
    {
        // 🔗 Bind XR target at runtime (PREFAB SAFE)
        // ResolveDependencies();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        grab = GetComponent<XRGrabInteractable>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = followDistance;
        agent.updateRotation = true;

        if (hitUI != null)
            hitUI.ResetCounter();
    }

    void Update()
    {
        // 🚫 If grabbed → stop everything
        if (grab != null && grab.isSelected)
        {
            StopEnemy();
            return;
        }

        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 🏃 Follow logic (UNCHANGED)
        if (distance > followDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            animator.SetBool("ISRUNNING", true);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("ISRUNNING", false);

            // ⚔️ Try hit when close
            TryHitPlayer();
        }
    }

    void StopEnemy()
    {
        agent.isStopped = true;
        animator.SetBool("ISRUNNING", false);
    }

    // 💥 Hit logic
    void TryHitPlayer()
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;
        currentHits++;

        if (hitUI != null)
            hitUI.UpdateCounter(currentHits);

        Debug.Log("Enemy Hit: " + currentHits);

        if (currentHits >= maxHits)
        {
            currentHits = 0;

            if (hitUI != null)
                hitUI.ResetCounter();

            if (respawnSystem != null)
                respawnSystem.ForceRespawn();
        }
    }

    // 🔗 XR target binding (runtime)
void ResolveDependencies()
{
    // 🎯 Find SpawnCube (scene object)
    if (target == null)
    {
        GameObject spawnCube = GameObject.FindWithTag("SpawnCube");
        if (spawnCube != null)
        {
            target = spawnCube.transform;
        }
    }

    // ♻ Find Respawn system
    if (respawnSystem == null)
    {
        respawnSystem = FindFirstObjectByType<DropAndRespawn>();
    }

    // 🧮 Find Hit UI
    if (hitUI == null)
    {
        hitUI = FindFirstObjectByType<HitCounterUI>();
    }

    Debug.Log($"Enemy bound → Target:{target} Respawn:{respawnSystem}");
}

}
