using UnityEngine;


public class PuzzleManagerXR : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets;
    public GameObject sphere;

    void Start()
    {
        sphere.SetActive(false);
    }

    void Update()
    {
        foreach (UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket in sockets)
        {
            if (!socket.hasSelection)
            {
                sphere.SetActive(false);
                return;
            }
        }

        sphere.SetActive(true);
    }
}