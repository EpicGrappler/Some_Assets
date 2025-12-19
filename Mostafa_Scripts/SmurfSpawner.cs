using UnityEngine;

public class SmurfSpawner : MonoBehaviour
{
    public GameObject smurfPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public int maxSmurfs = 5;
    public float spawnInterval = 2f;

    [Header("Follow Settings")]
    public Transform followTarget;
    public float followDistance = 1.2f;
    public float moveSpeed = 2.5f;
    private static SmurfSpawner instance;

    // 🔥 BOSS TRIGGER (ADDED)
    [Header("Boss Trigger")]
    public BossSpawner bossSpawner;

    private int currentSmurfs = 0;
    private bool spawningEnabled = true;

    private bool bossTriggered = false;

void Start()
{
    if (instance != this) return;

    Debug.Log("SmurfSpawner START on: " + gameObject.name);
    InvokeRepeating(nameof(SpawnSmurf), 0f, spawnInterval);
}




void Awake()
{
    if (instance != null && instance != this)
    {
        Destroy(gameObject);
        return;
    }

    instance = this;
}

    void SpawnSmurf()
    {
        if (!spawningEnabled) return;
        if (currentSmurfs >= maxSmurfs) return;
        if (spawnPoints.Length == 0 || smurfPrefab == null) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject smurf = Instantiate(smurfPrefab, point.position, Quaternion.identity);

        currentSmurfs++;

        float scale = Random.Range(0.5f, 1.5f);
        smurf.transform.localScale = Vector3.one * scale;

        Smurf smurfScript = smurf.GetComponent<Smurf>();
        smurfScript.points = Mathf.RoundToInt(scale * 2);
        smurfScript.spawner = this;

        PlayerFollowNavMesh follow = smurf.GetComponent<PlayerFollowNavMesh>();
        if (follow != null)
        {
            follow.target = followTarget;
            follow.followDistance = followDistance;
            follow.moveSpeed = moveSpeed;
        }
    }

    // 🔥 THIS IS THE ONLY METHOD THAT CONTROLS BOSS SPAWN
public int scoreToTriggerBoss = 10;

public void OnSmurfDestroyed()
{
    currentSmurfs--;

    if (!bossTriggered && ScoreManager.Instance.score >= ScoreManager.Instance.unlockScore)
    {
        bossTriggered = true;
        bossSpawner.SpawnBoss();
    }
}



    // 🔥 CALLED WHEN BOSS DIES
    public void StopSpawning()
    {
        spawningEnabled = false;
        CancelInvoke(nameof(SpawnSmurf));
    }
}
