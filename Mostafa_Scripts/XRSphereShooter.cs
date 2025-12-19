using UnityEngine;

public class XRSphereShooter : MonoBehaviour
{
    public GameObject spherePrefab;
    public float shootForce = 15f;
    public float fireRate = 0.25f;

    private float nextFireTime;

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (!spherePrefab)
        {
            Debug.LogError("Sphere Prefab NOT assigned!");
            return;
        }

        GameObject bullet = Instantiate(
            spherePrefab,
            transform.position + transform.forward * 0.5f,
            transform.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * shootForce;

        Destroy(bullet, 5f);
    }
}
