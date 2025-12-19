using UnityEngine;

public class LightStateForAll : MonoBehaviour
{
    public enum ConditionType
    {
        VaseDestroyed,
        KeyCollected,
        KeyCollided,
        KeyTomb,
    }

    [Header("Condition")]
    public ConditionType condition;

    [Header("Lights")]
    public GameObject redLight;
    public GameObject greenLight;
    
    void Awake()
    {
        bool isSolved = GetConditionState();

        Debug.Log($"🟢 {gameObject.name} | Condition {condition} = {isSolved}");

        redLight.SetActive(!isSolved);
        greenLight.SetActive(isSolved);
    }

    bool GetConditionState()
    {
        switch (condition)
        {
            case ConditionType.VaseDestroyed:
                return GameState.IsVaseDestroyed;

            case ConditionType.KeyCollected:
                return GameState.IsKeyCollected;

            case ConditionType.KeyCollided:
                return GameState.IsKeyCollided_Tarek;

            case ConditionType.KeyTomb:
                return GameState.IsKeyCollodedTomb;
                
            default:
                return false;
        }
    }
}
