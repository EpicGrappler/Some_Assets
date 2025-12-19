using UnityEngine;

public class FollowXROriginOnGround : MonoBehaviour
{
    public Transform xrOrigin;

    void Update()
    {
        if (xrOrigin == null) return;

        Vector3 pos = xrOrigin.position;
        pos.y = 0; // مهم جدًا: يخليه على الأرض
        transform.position = pos;
    }
}
