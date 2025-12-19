using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneSwitcherForAll : MonoBehaviour
{
    public enum ConditionType
    {
        None,           // door always usable
        VaseDestroyed,
        KeyCollected,
        KeyCollided_Tarek,
        keyTomb,
    }

    [Header("Scene")]
    public string sceneToLoad;

    [Header("Lock Condition")]
    public ConditionType lockCondition = ConditionType.None;

    void Awake()
    {
        if (IsLocked())
        {
            Debug.Log($"Door locked ({lockCondition}) on {gameObject.name}");
            enabled = false; // disable interaction
        }
    }

    void Update()
    {
        if (!enabled) return;
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
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

    bool IsLocked()
    {
        switch (lockCondition)
        {
            case ConditionType.VaseDestroyed:
                return GameState.IsVaseDestroyed;

            case ConditionType.KeyCollected:
                return GameState.IsKeyCollected;

            case ConditionType.KeyCollided_Tarek:
                return GameState.IsKeyCollided_Tarek;

            case ConditionType.keyTomb:
                return GameState.IsKeyCollodedTomb;

            default:
                return false;
        }
    }
}
