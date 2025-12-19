using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;      // The player to follow
    public float speed = 3f;      // Move speed
    public float rotationSpeed = 10f; // Turning smoothness
    public float stopDistance = 1.5f; // Distance to keep from the player

    void Update()
    {
        if (player == null) return;

        // Distance to player
        float distance = Vector3.Distance(transform.position, player.position);

        // Rotate toward player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move toward player if too far
        if (distance > stopDistance)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
