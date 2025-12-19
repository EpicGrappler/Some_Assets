using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting; // for new Input System mouse click

public class DoorSceneSwitcher : MonoBehaviour
{
    public string sceneToLoad; // set this in the inspector
    public Canvas poster;
    void Start()
    {
        // 🔥 NEW: disable scene switching permanently if game is finished
        if (GameState.GameFinished)
        {
            Destroy(this); // removes ONLY this script
        }
    }

    private void Update()
    {
        // 🔥 SAFETY: if finished, do nothing
        if (GameState.GameFinished)
            return;

        // If left mouse click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}
