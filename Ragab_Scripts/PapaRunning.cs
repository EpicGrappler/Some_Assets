// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class PlayerAnim : MonoBehaviour
// {
//     public Transform target;
//     public float speed = 2.5f;
//     public float stopDistance = 1.2f;
//     public float rotationSpeed = 8f;

//     Animator animator;
//     XRGrabInteractable grab;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         grab = GetComponent<XRGrabInteractable>();
//     }

//     void Update()
//     {
//         // 🚫 If grabbed → do NOTHING
//         if (grab != null && grab.isSelected)
//         {
//             animator.SetBool("ISRUNNING", false);
//             return;
//         }

//         if (target == null) return;

//         float distance = Vector3.Distance(transform.position, target.position);

//         if (distance > stopDistance)
//         {
//             animator.SetBool("ISRUNNING", true);

//             Vector3 direction = (target.position - transform.position).normalized;
//             direction.y = 0f;

//             Quaternion lookRotation = Quaternion.LookRotation(direction);
//             transform.rotation = Quaternion.Slerp(
//                 transform.rotation,
//                 lookRotation,
//                 rotationSpeed * Time.deltaTime
//             );

//             transform.Translate(Vector3.forward * speed * Time.deltaTime);
//         }
//         else
//         {
//             animator.SetBool("ISRUNNING", false);
//         }
//     }
// }


// using UnityEngine;


// public class PlayerAnim : MonoBehaviour
// {
//     public Transform target;              // الكاميرا / الـXR Rig
//     public float speed = 2.5f;
//     public float stopDistance = 5f;     // أقل مسافة لو قربت يبدأ يهرب (أو يفضل يهرب حسب اللي تختاره)
//     public float rotationSpeed = 8f;
//     public float safeDistance = 3.5f;     // مسافة "أمان" لو وصلها يوقف هروب (اختياري)

//     Animator animator;
//     UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
//     }

//     void Update()
//     {
//         // 🚫 لو متشافك (grabbed) → وقف حركة/انيميشن
//         if (grab != null && grab.isSelected)
//         {
//             animator.SetBool("ISRUNNING", false);
//             return;
//         }

//         if (target == null) return;

//         float distance = Vector3.Distance(transform.position, target.position);

//         // ✅ لو قريب من الكاميرا → اهرب
//         // ولو عايز يفضل يهرب دايمًا شيل شرط stopDistance وخليه دايمًا true
//         if (distance < safeDistance) 
//         {
//             // لو بعيد كفاية (مثلاً أكتر من safeDistance) ممكن توقفه
//             // هنا عاملها: طالما أقل من safeDistance هيجري، لما يبعد يوقف
//             animator.SetBool("ISRUNNING", true);

//             Vector3 direction = (transform.position - target.position).normalized; // 👈 بعيد عن الكاميرا
//             direction.y = 0f;

//             // لو حصل اتجاه شبه صفر (نادر) امنعه
//             if (direction.sqrMagnitude < 0.001f)
//                 direction = transform.forward;

//             Quaternion lookRotation = Quaternion.LookRotation(direction);
//             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

//             transform.Translate(Vector3.forward * speed * Time.deltaTime);
//         }
//         else
//         {
//             animator.SetBool("ISRUNNING", false);
//         }
//     }
// }
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class PlayerAnim : MonoBehaviour
// {
//     public Transform target;          // XR Camera / XR Rig
//     public float runSpeed = 3.5f;
//     public float safeDistance = 2.5f;   // distance to start escaping

//     private NavMeshAgent agent;
//     private Animator animator;
//     private XRGrabInteractable grab;

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponent<Animator>();
//         grab = GetComponent<XRGrabInteractable>();

//         agent.speed = runSpeed;
//         agent.updateRotation = true;
//     }

//     void Update()
//     {
//         // 🚫 If grabbed → stop everything
//         if (grab != null && grab.isSelected)
//         {
//             agent.isStopped = true;
//             animator.SetBool("ISRUNNING", false);
//             return;
//         }

//         if (target == null) return;

//         float distance = Vector3.Distance(transform.position, target.position);

//         // 🏃 Escape if too close
//         if (distance < safeDistance)
//         {
//             animator.SetBool("ISRUNNING", true);
//             agent.isStopped = false;

//             // direction AWAY from target
//             Vector3 fleeDirection = (transform.position - target.position).normalized;
//             Vector3 fleeTarget = transform.position + fleeDirection * 5f;

//             // snap destination to NavMesh
//             if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
//             {
//                 agent.SetDestination(hit.position);
//             }
//         }
//         else
//         {
//             // 🛑 Safe → stop
//             agent.isStopped = true;
//             animator.SetBool("ISRUNNING", false);
//         }
//     }
// }


// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class PapaRunning : MonoBehaviour
// {
//     public Transform target;          // XR Camera / XR Rig
//     public float runSpeed = 3.5f;
//     public float safeDistance = 2.5f; // distance to start escaping

//     private NavMeshAgent agent;
//     private Animator animator;
//     private XRGrabInteractable grab;


//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponent<Animator>();
//         grab = GetComponent<XRGrabInteractable>();

//         agent.speed = runSpeed;

//         // 🔑 مهم جدًا للسلاسة
//         agent.updateRotation = false;
//     }

//     void Update()
//     {
//         // 🚫 If grabbed → stop everything
//         if (grab != null && grab.isSelected)
//         {
//             agent.isStopped = true;
//             animator.SetBool("ISRUNNING", false);
//             return;
//         }

//         if (target == null) return;

//         float distance = Vector3.Distance(transform.position, target.position);

//         // 🏃 Escape if too close
//         if (distance < safeDistance)
//         {
//             animator.SetBool("ISRUNNING", true);
//             agent.isStopped = false;

//             // direction AWAY from target (زي كودك الأصلي)
//             Vector3 fleeDirection = (transform.position - target.position).normalized;
//             Vector3 fleeTarget = transform.position + fleeDirection * 5f;

//             // snap destination to NavMesh
//             if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
//             {
//                 agent.SetDestination(hit.position);
//             }
//         }
//         else
//         {
//             // 🛑 Safe → stop
//             agent.isStopped = true;
//             animator.SetBool("ISRUNNING", false);
//         }
//     }

//     // 🌀 دوران ناعم بدل دوران الـ NavMesh
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
// }




using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PapaRunning : MonoBehaviour
{
    public Transform target;          
    public float runSpeed = 3.5f;
    public float safeDistance = 2.5f;

    [Header("Fear Settings")]
    public float panicDuration = 4f;     // ⏱ يكمل جري حتى لو وقفت
    public float fleeDistance = 6f;      // يبعد قد إيه
    public float directionChangeTime = 1.2f; // يغيّر اتجاهه كل شوية

    private NavMeshAgent agent;
    private Animator animator;
    private XRGrabInteractable grab;

    private float panicTimer;
    private float directionTimer;
    private bool isFleeing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        grab = GetComponent<XRGrabInteractable>();

        agent.speed = runSpeed;
        agent.updateRotation = false;
    }

    void Update()
    {
        // لو اتشد → يقف
        if (grab != null && grab.isSelected)
        {
            StopRunning();
            return;
        }

        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 😱 أول ما تقرب
        if (distance < safeDistance)
        {
            isFleeing = true;
            panicTimer = panicDuration;
        }

        if (isFleeing)
        {
            panicTimer -= Time.deltaTime;
            directionTimer -= Time.deltaTime;

            animator.SetBool("ISRUNNING", true);
            agent.isStopped = false;

            // غيّر الاتجاه كل شوية
            if (directionTimer <= 0f || agent.remainingDistance < 0.5f)
            {
                SetNewFleeDestination();
                directionTimer = directionChangeTime;
            }

            // هدي خلاص
            if (panicTimer <= 0f)
            {
                isFleeing = false;
                StopRunning();
            }
        }
    }

    void SetNewFleeDestination()
    {
        // اتجاه بعيد عن اللاعب + شوية عشوائية
        Vector3 awayDir = (transform.position - target.position).normalized;
        Vector3 randomSide = Random.insideUnitSphere;
        randomSide.y = 0;

        Vector3 fleeDir = (awayDir + randomSide * 0.4f).normalized;
        Vector3 fleeTarget = transform.position + fleeDir * fleeDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void StopRunning()
    {
        agent.isStopped = true;
        animator.SetBool("ISRUNNING", false);
    }

    void LateUpdate()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rot,
                Time.deltaTime * 8f
            );
        }
    }
}


