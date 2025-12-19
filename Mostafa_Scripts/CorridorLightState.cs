using UnityEngine;

public class CorridorLightState : MonoBehaviour
{
    public GameObject redLight;
    public GameObject greenLight;

    void Start()
    {
        if (GameState.GameFinished)
        {
            redLight.SetActive(false);
            greenLight.SetActive(true);
        }
        else
        {
            redLight.SetActive(true);
            greenLight.SetActive(false);
        }
    }
}
