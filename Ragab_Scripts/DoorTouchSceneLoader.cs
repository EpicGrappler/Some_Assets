using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTouchSceneLoader : MonoBehaviour
{
    [Header("References")]
    public GameObject key;   // اسحب المفتاح هنا
    // public GameObject 
    [Header("Scene Settings")]
    public string sceneToLoad = "Scene2";

    private bool loaded = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (loaded) return;


        if (other.CompareTag("TombKey"))
        {
            loaded = true;

            GameState.IsKeyCollodedTomb = true;
            SceneManager.LoadScene(sceneToLoad);
        }
        
        else if (other.gameObject == key)
        {
            loaded = true;
            GameState.IsKeyCollected = true;
            SceneManager.LoadScene(sceneToLoad);
        }
        

    }
}
