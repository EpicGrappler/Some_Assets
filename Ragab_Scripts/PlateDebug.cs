using UnityEngine;

public class PlateDebug : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PLATE WORKS - Entered by: " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("PLATE STAY - Still touching: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("PLATE EXIT - Left by: " + other.name);
    }
}