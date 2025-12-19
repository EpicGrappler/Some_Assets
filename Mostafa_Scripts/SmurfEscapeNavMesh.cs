using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SmurfEscapeNavMesh : MonoBehaviour
{
    public Transform target;          // XR Camera / XR Rig
    public float runSpeed = 3.5f;
    public float safeDistance = 4f;   // distance to start escaping

    private NavMeshAgent agent;
    private Animator animator;
    private XRGrabInteractable grab;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        grab = GetComponent<XRGrabInteractable>();

        agent.speed = runSpeed;
        agent.updateRotation = true;
    }

    void Update()
    {
        // 🚫 If grabbed → stop everything
        if (grab != null && grab.isSelected)
        {
            agent.isStopped = true;
            animator.SetBool("ISRUNNING", false);
            return;
        }

        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 🏃 Escape if too close
        if (distance < safeDistance)
        {
            animator.SetBool("ISRUNNING", true);
            agent.isStopped = false;

            // direction AWAY from target
            Vector3 fleeDirection = (transform.position - target.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * 5f;

            // snap destination to NavMesh
            if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            // 🛑 Safe → stop
            agent.isStopped = true;
            animator.SetBool("ISRUNNING", false);
        }
    }
}
