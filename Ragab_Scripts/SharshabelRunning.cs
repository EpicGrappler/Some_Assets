// using UnityEngine;
// using UnityEngine.AI;
// using Unity.XR.CoreUtils;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class SharshabelRunning : MonoBehaviour
// {
//     [Header("Chase")]
//     public Transform target;          // XR Origin (مش الكاميرا)
//     public float runSpeed = 3.5f;
//     public float chaseDistance = 10f;
//     public float stopDistance = 1.2f;

//     [Header("Hits System")]
//     public int maxHits = 3;
//     private int currentHits = 0;
//     public float hitCooldown = 1f;
//     private float lastHitTime;

//     public DropAndRespawn respawnSystem;

//     private NavMeshAgent agent;
//     private Animator animator;
//     private XRGrabInteractable grab;

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponent<Animator>();
//         grab = GetComponent<XRGrabInteractable>();

//         agent.speed = runSpeed;
//         agent.stoppingDistance = stopDistance;   // ⭐ مهم
//         agent.updateRotation = false;
//     }

//     void Update()
//     {
//         // 🚫 لو ماسكه بإيدك
//         if (grab != null && grab.isSelected)
//         {
//             StopEnemy();
//             return;
//         }

//         if (target == null) return;

//         // نخلي العدو يجري على الأرض مش على راسك
//         Vector3 targetPos = target.position;
//         targetPos.y = transform.position.y;

//         float distance = Vector3.Distance(transform.position, targetPos);

//         if (distance <= chaseDistance)
//         {
//             animator.SetBool("ISRUNNING", true);
//             agent.isStopped = false;
//             agent.SetDestination(targetPos);
//         }
//         else
//         {
//             StopEnemy();
//         }
//     }

//     void StopEnemy()
//     {
//         agent.isStopped = true;
//         animator.SetBool("ISRUNNING", false);
//     }

//     // 🌀 دوران ناعم
//     void LateUpdate()
//     {
//         if (agent.velocity.magnitude > 0.1f)
//         {
//             Quaternion targetRot = Quaternion.LookRotation(agent.velocity.normalized);
//             transform.rotation = Quaternion.Slerp(
//                 transform.rotation,
//                 targetRot,
//                 Time.deltaTime * 8f
//             );
//         }
//     }

//     // 💥 لما يلمسك
//     private void OnTriggerEnter(Collider other)
//     {
//         if (Time.time - lastHitTime < hitCooldown) return;

//         if (other.GetComponentInParent<XROrigin>() != null)
//         {
//             lastHitTime = Time.time;
//             currentHits++;

//             Debug.Log("Enemy Hit: " + currentHits);

//             if (currentHits >= maxHits)
//             {
//                 currentHits = 0;
//                 respawnSystem.ForceRespawn();
//             }
//         }
//     }
// }



using UnityEngine;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SharshabelRunning : MonoBehaviour
{
    [Header("Chase Settings")]
    public Transform target;              // XR Origin (XR Rig)
    public float runSpeed = 3.5f;
    public float chaseDistance = 10f;
    public float stopDistance = 1.2f;

    [Header("Hit System")]
    public int maxHits = 3;
    public float hitCooldown = 1f;

    public DropAndRespawn respawnSystem;  // سكربت السقوط / الرجوع
    public HitCounterUI hitUI;            // سكربت العداد (UI)

    private int currentHits = 0;
    private float lastHitTime;

    private NavMeshAgent agent;
    private Animator animator;
    private XRGrabInteractable grab;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // لو الأنيميشن على Child
        animator = GetComponentInChildren<Animator>();

        grab = GetComponent<XRGrabInteractable>();

        agent.speed = runSpeed;
        agent.stoppingDistance = stopDistance;
        agent.updateRotation = false;

        // نبدأ العداد من صفر
        if (hitUI != null)
            hitUI.ResetCounter();
    }

    void Update()
    {
        // 🚫 لو اللاعب ماسك العدو
        if (grab != null && grab.isSelected)
        {
            StopEnemy();
            return;
        }

        if (target == null) return;

        // نخلي الجري على الأرض (مش على راس اللاعب)
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, targetPos);

        // 🏃 الجري
        if (distance <= chaseDistance)
        {
            animator.SetBool("ISRUNNING", true);
            agent.isStopped = false;
            agent.SetDestination(targetPos);
        }
        else
        {
            StopEnemy();
        }

        // ⚔️ الهجوم بالمسافة
        if (distance <= stopDistance)
        {
            TryHitPlayer();
        }
    }

    void StopEnemy()
    {
        agent.isStopped = true;
        animator.SetBool("ISRUNNING", false);
    }

    // 🌀 دوران ناعم
    void LateUpdate()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 8f
            );
        }
    }

    // 💥 حساب الضرب
    void TryHitPlayer()
    {
        if (Time.time - lastHitTime < hitCooldown) return;

        lastHitTime = Time.time;
        currentHits++;

        // تحديث العداد
        if (hitUI != null)
            hitUI.UpdateCounter(currentHits);

        Debug.Log("Enemy Hit: " + currentHits);

        if (currentHits >= maxHits)
        {
            currentHits = 0;

            // تصفير العداد
            if (hitUI != null)
                hitUI.ResetCounter();

            // Respawn
            if (respawnSystem != null)
                respawnSystem.ForceRespawn();
        }
    }
}
