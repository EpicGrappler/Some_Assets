using UnityEngine;

public class Smurf : MonoBehaviour
{
    public int points = 1;
    public int damage = 10;
    public SmurfSpawner spawner;

    private bool isDead = false; // 🔥 CRITICAL

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Projectile"))
        {
            isDead = true;

            ScoreManager.Instance.AddScore(points);

            if (spawner != null)
                spawner.OnSmurfDestroyed(); // ✅ CALLED ONCE

            Destroy(other.gameObject); // bullet
            Destroy(gameObject);       // smurf
        }
    }
}
