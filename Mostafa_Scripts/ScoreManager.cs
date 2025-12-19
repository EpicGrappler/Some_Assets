using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score;

    [Header("Boss Unlock")]
    public int unlockScore = 10;
    public GameObject cage;

    [Header("UI")]
    public TextMeshProUGUI smurfCounterText;

    // 🔴 ADD THIS
    public TextMeshProUGUI bossHPText;

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();

        if (score >= unlockScore)
        {
            cage.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (smurfCounterText != null)
        {
            smurfCounterText.text = "Score: " + score;
        }
    }

    // 🔴 ADD THIS METHOD
    public void UpdateBossHP(int hp)
    {
        if (bossHPText != null)
        {
            bossHPText.text = "Boss HP: " + hp;
        }
    }
}
