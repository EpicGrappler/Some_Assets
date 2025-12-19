using TMPro;
using UnityEngine;

public class HitCounterUI : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public int maxHits = 3;

    public void UpdateCounter(int currentHits)
    {
        counterText.text = currentHits + " / " + maxHits;
    }

    public void ResetCounter()
    {
        counterText.text = "0 / " + maxHits;
    }
}
