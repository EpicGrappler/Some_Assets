using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // for new Input System mouse click

public class DoorSceneSwitcher : MonoBehaviour
{
    public string sceneToLoad; // set this in the inspector

    private void Update()
    {
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
