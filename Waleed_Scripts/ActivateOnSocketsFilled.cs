using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ActivateOnSocketsFilled : MonoBehaviour
{
    public XRSocketInteractor[] sockets;

    [Header("Objects To Activate")]
    public GameObject[] objectsToActivate;

    void Start()
    {
        // ???? ?? ??????? ?? ?????
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(_ => CheckSockets());
            socket.selectExited.AddListener(_ => CheckSockets());
        }
    }

    void CheckSockets()
    {
        foreach (var socket in sockets)
        {
            if (!socket.hasSelection)
            {
                SetObjectsActive(false);
                return;
            }
        }

        SetObjectsActive(true);
    }

    void SetObjectsActive(bool value)
    {
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(value);
        }
    }
}