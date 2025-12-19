using UnityEngine;

public class BossSmurf : MonoBehaviour
{
    public int health = 50;
    public GameObject keyPrefab;
    public Canvas poster;
    [HideInInspector]
    public SmurfSpawner smurfSpawner;

    void Start()
    {
        // 🔴 INITIALIZE UI
        ScoreManager.Instance.UpdateBossHP(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Projectile")) return;

        health--;

        // 🔴 UPDATE UI
        ScoreManager.Instance.UpdateBossHP(health);

        if (health <= 0)
        {
            if (smurfSpawner != null)
                smurfSpawner.StopSpawning();

            GameState.GameFinished = true;
        if (!GameState.PosterHidden)
        {
            poster.gameObject.SetActive(false);
            GameState.PosterHidden = true;
        }
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
