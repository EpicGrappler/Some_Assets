using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPoint;

    [Header("Follow Settings")]
    public Transform followTarget;
    public float followDistance = 2.0f;
    public float moveSpeed = 1.5f;

    [Header("References")]
    public SmurfSpawner smurfSpawner;

    private bool bossSpawned = false;

    public void SpawnBoss()
    {
        if (bossSpawned) return;

        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        bossSpawned = true;

        // 🔹 Assign follow behavior
        PlayerFollowNavMesh follow = boss.GetComponent<PlayerFollowNavMesh>();
        if (follow != null)
        {
            follow.target = followTarget;
            follow.followDistance = followDistance;
            follow.moveSpeed = moveSpeed;
        }

        // 🔹 Tell boss about spawner
        BossSmurf bossSmurf = boss.GetComponent<BossSmurf>();
        if (bossSmurf != null)
        {
            bossSmurf.smurfSpawner = smurfSpawner;
        }
    }
}
