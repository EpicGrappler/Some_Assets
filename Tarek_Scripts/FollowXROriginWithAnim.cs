using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FollowXROriginWithAnim : MonoBehaviour
{
    [Header("Target (XR)")]
    [Tooltip("Assign the XR Origin (preferred) or the VR Camera/Head transform.")]
    public Transform xrOrigin;            // prefer XR Origin (camera rig root)
    public Transform cameraTransform;     // optional: main camera (head) if xrOrigin null

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float rotationSpeed = 8f;
    public float stopDistance = 1.2f;     // how close the smurf gets
    public float followHeightOffset = 0f;  // optional vertical offset on target

    [Header("Animation")]
    public Animator animator;             // animator with walk/idle
    [Tooltip("If true uses 'Speed' float parameter. If false uses 'isWalking' bool parameter")]
    public bool useFloatSpeedParameter = true;
    public string speedParam = "Speed";    // float param (recommended: blend tree)
    public string walkBoolParam = "isWalking";

    [Header("Extras")]
    public bool useRootMotion = false;    // set true only if the animation uses root motion
    public float movementSmoothing = 0.15f;

    // runtime
    CharacterController cc;
    Vector3 currentVelocity;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (xrOrigin == null && cameraTransform != null)
        {
            // it's okay — we'll follow the camera position if xrOrigin not provided
        }
    }

    void Update()
    {
        Transform targetSource = xrOrigin != null ? xrOrigin : cameraTransform;
        if (targetSource == null) return;

        // Build the target position but keep the Smurf's Y (ground) + optional offset
        Vector3 targetPos = targetSource.position;
        targetPos.y = transform.position.y + followHeightOffset;

        Vector3 toTarget = targetPos - transform.position;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float distance = toTargetXZ.magnitude;

        // Rotate smoothly towards target only on Y axis
        if (toTargetXZ.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toTargetXZ.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // Movement
        Vector3 desiredMove = Vector3.zero;
        float desiredSpeed = 0f;

        if (distance > stopDistance)
        {
            desiredMove = transform.forward * moveSpeed; // forward is already facing target
            desiredSpeed = moveSpeed;
        }

        // Smooth movement (damp)
        Vector3 smoothMove = Vector3.SmoothDamp(cc.velocity, desiredMove, ref currentVelocity, movementSmoothing);

        // If using CharacterController, move with cc.Move (keeps collisions)
        if (cc != null)
        {
            Vector3 moveDelta = smoothMove * Time.deltaTime;
            // keep gravity by adding a small downward force if needed (optional)
            if (!useRootMotion)
            {
                cc.Move(moveDelta);
            }
        }
        else
        {
            // fallback: change transform.position directly
            transform.position += smoothMove * Time.deltaTime;
        }

        // Animator control
        if (animator != null)
        {
            if (useFloatSpeedParameter)
            {
                // map desired speed to [0..1] for convenient blend trees, or use raw value
                animator.SetFloat(speedParam, desiredSpeed);
            }
            else
            {
                animator.SetBool(walkBoolParam, desiredSpeed > 0.01f);
            }
        }
    }

    // If you want to use root motion, this forwards root motion to the CharacterController
    void OnAnimatorMove()
    {
        if (!useRootMotion || animator == null) return;

        Vector3 delta = animator.deltaPosition;
        if (cc != null)
        {
            cc.Move(delta);
        }
        else
        {
            transform.position += delta;
        }
        transform.rotation = animator.rootRotation;
    }
}
