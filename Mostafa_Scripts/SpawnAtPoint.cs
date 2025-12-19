using UnityEngine;

public class SpawnAtPoint : MonoBehaviour
{
    void Start()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            transform.rotation = spawn.transform.rotation;
        }
        else
        {
            Debug.LogWarning("No SpawnPoint found in the scene.");
        }
    }
}
