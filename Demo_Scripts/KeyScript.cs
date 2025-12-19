using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // This runs as soon as the Vase calls keyInside.SetActive(true)
    void OnEnable()
    {
        // Give the key a little "pop" upward or random rotation when it appears
        if (rb != null)
        {
            rb.linearVelocity = Vector3.up * 2f; 
            rb.AddTorque(Random.insideUnitSphere * 5f);
        }
    }
}