// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class PlayerAnim1 : MonoBehaviour
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
// using UnityEngine.AI;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class PlayerAnim1 : MonoBehaviour
// {
//     public Transform target;          // XR Camera / XR Rig
//     public float runSpeed = 3.5f;
//     public float safeDistance = 100f;   // distance to start escaping

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
//             Vector3 fleeDirection = (target.position-transform.position).normalized;
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


using UnityEngine;
using UnityEngine.AI;

public class PlayerAnim1 : MonoBehaviour
{
    public Transform target;      // 🧍 NPC2 (اللي بيجري منك)
    public Transform player;      // 👤 أنت

    public float runSpeed = 3.5f;
    public float catchDistance = 1.2f;  // المسافة لامساك NPC2
    public float fleeDistance = 5f;     // المسافة للهروب بعيد عن اللاعب

    private NavMeshAgent agent;
    private Animator animator;

    private bool hasCaught = false;  // Boolean لمعرفة إذا كان NPC1 مسك NPC2

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = runSpeed;          // سرعة الحركة
        agent.updateRotation = true;     // تحديث التدوير بشكل طبيعي
        agent.stoppingDistance = catchDistance;  // المسافة التي يتوقف عندها الـ NavMeshAgent

        animator.applyRootMotion = false;  // تأكد أن الحركة مش مرتبطة بـ Root Motion في الأنيميشن

        // تأكد أن الـ agent يقدر يتجنب العقبات
        agent.avoidancePriority = 50;  // تحديد أولوية التجنب (من 0 إلى 99)
    }

    void Update()
    {
        if (target == null || player == null) return;

        // إذا لم يتم مسك NPC2 بعد، تابع المطاردة
        if (!hasCaught)
            ChaseTarget();      // 🏃 ورا NPC2
        else
            FleeFromPlayer();  // 🏃 بعيد عنك بعد ما مسكنا NPC2
    }

    // ================= Chase NPC2 =================
    void ChaseTarget()
    {
        animator.SetBool("ISRUNNING", true);   // شغل الأنيميشن الخاص بالجري
        agent.isStopped = false;   // تأكد أن الـ NavMeshAgent شغال

        agent.SetDestination(target.position);

        // إذا قربنا من NPC2 بشكل كافي (بالمسافة المحددة catchDistance)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasCaught = true; // لو المسافة قليلة بما فيه الكفاية، تم مسك NPC2

            // وقف NPC2
            NavMeshAgent targetAgent = target.GetComponent<NavMeshAgent>();
            if (targetAgent != null)
                targetAgent.isStopped = true;  // وقفه تمامًا

            // بعد ما مسكناه، خلي NPC2 يثبت في مكانه أو تمسكه في مكان آخر
            target.SetParent(transform);    // اجعل NPC2 جزء من NPC1 (تمسكه)
            target.localPosition = Vector3.zero;  // خلي مكانه متناسب مع يد NPC1
            target.localRotation = Quaternion.identity;  // ضبط التدوير
        }
    }

    // ================= Flee From Player =================
    void FleeFromPlayer()
    {
        // احسب اتجاه الهروب بعيدًا عن اللاعب
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;  // مكان الهروب

        // إذا كان المكان الجديد صالح على الـ NavMesh، خلي NPC1 يتحرك في هذا الاتجاه
        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        animator.SetBool("ISRUNNING", true);   // شغل الأنيميشن الخاص بالجري
        agent.isStopped = false;   // خليه يركض للوجهة المحددة
    }
}


