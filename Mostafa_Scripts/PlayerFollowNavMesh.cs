using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlayerFollowNavMesh : MonoBehaviour
{
    public Transform target;
    public float followDistance = 1.2f;
    public float moveSpeed = 2.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private XRGrabInteractable grab;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        grab = GetComponent<XRGrabInteractable>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = followDistance;
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
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            animator.SetBool("ISRUNNING", false);
        }
    }
}
