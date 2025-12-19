using UnityEngine;

public class VaseScript : MonoBehaviour
{
    public GameObject keyInside; 
    public float breakForceThreshold = 1.1f; 

    void Start()
    {
        // Force the key to be "locked" at the start
        if (keyInside != null)
        {
            Rigidbody keyRb = keyInside.GetComponent<Rigidbody>();
            if (keyRb != null) keyRb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakForceThreshold)
        {
            BreakVase();
        }
    }

    void BreakVase()
    {
        if (keyInside != null)
        {
            // 1. Unparent so it doesn't disappear with the vase
            keyInside.transform.SetParent(null); 
            
            // 2. Enable Physics
            Rigidbody keyRb = keyInside.GetComponent<Rigidbody>();
            if (keyRb != null) keyRb.isKinematic = false;

            // 3. Enable Interaction (This turns on your grab script)
            // Replace 'MonoBehaviour' with your specific grab script name if needed
            var grabScript = keyInside.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(); 
            if (grabScript != null) grabScript.enabled = true;

            keyInside.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}