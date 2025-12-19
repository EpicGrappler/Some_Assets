using UnityEngine;

public class EnemyTargetBinder : MonoBehaviour
{
    [Header("Scene Target")]
    public Transform target;   // SpawnCube (scene object)

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("EnemyTargetBinder: Target not assigned");
            return;
        }

        EnemyFollowAndHit[] enemies =
            FindObjectsByType<EnemyFollowAndHit>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            enemy.target = target;
        }

        Debug.Log($"Bound {enemies.Length} enemies to target {target.name}");
    }
}
