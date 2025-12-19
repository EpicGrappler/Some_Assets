using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WinningMessage : MonoBehaviour
{
    public GameObject Ragab_Wall;
    public GameObject Ragab_Door;
    public GameObject Ragab_Window;
    public GameObject MonaZaky;
    private bool unlocked = false; // prevent repeating Destroy()
    public TextMeshPro message;
    void Update()
    {
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (unlocked) 
        return;

        if (
            GameState.GameFinished &&
            GameState.IsVaseDestroyed &&
            GameState.IsKeyCollodedTomb &&
            GameState.IsKeyCollided_Tarek &&
            GameState.IsKeyCollected
        )
        {
            Destroy(Ragab_Door);
            Destroy(Ragab_Wall);
            Destroy(Ragab_Window);
            message.gameObject.SetActive(true);
            MonaZaky.SetActive(true);
            
            unlocked = true;
        }
    }
}
