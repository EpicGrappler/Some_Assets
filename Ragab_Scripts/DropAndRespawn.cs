// using UnityEngine;
// using Unity.XR.CoreUtils;
// using System.Collections;

// public class DropAndRespawn : MonoBehaviour
// {
//     public Collider floorCollider;
//     public XROrigin xrOrigin;
//     public Transform respawnPoint;

//     public float fallTime = 2f;

//     private bool triggered = false;

//     private void OnTriggerEnter(Collider other)
//     {
//         if (triggered) return;

//         if (other.GetComponentInParent<XROrigin>() != null)
//         {
//             triggered = true;
//             StartCoroutine(DropSequence());
//         }
//     }

//     IEnumerator DropSequence()
//     {
//         // 1️⃣ الأرض تقع
//         floorCollider.enabled = false;

//         // 2️⃣ نستنى السقوط
//         yield return new WaitForSeconds(fallTime);

//         // 3️⃣ نرجع XR Origin لمكان البداية
//         xrOrigin.transform.position = respawnPoint.position;
//         xrOrigin.transform.rotation = respawnPoint.rotation;

//         // 4️⃣ نعمل Reset للكاميرا
//         xrOrigin.CameraYOffset = 0f;
//         xrOrigin.MoveCameraToWorldLocation(respawnPoint.position);

//         // 5️⃣ نرجّع الأرض
//         floorCollider.enabled = true;

//         triggered = false;
//     }
// }



// using UnityEngine;
// using Unity.XR.CoreUtils;
// using System.Collections;

// public class DropAndRespawn : MonoBehaviour
// {
//     public Collider floorCollider;
//     public XROrigin xrOrigin;
//     public Transform respawnPoint;

//     public float fallTime = 2f;

//     private bool triggered = false;

//     private void OnTriggerEnter(Collider other)
//     {
//         if (triggered) return;

//         if (other.GetComponentInParent<XROrigin>() != null)
//         {
//             triggered = true;
//             StartCoroutine(DropSequence());
//         }
//     }

//     // ⭐⭐⭐ الدالة المطلوبة للعدو
//     public void ForceRespawn()
//     {
//         if (triggered) return;

//         triggered = true;
//         StopAllCoroutines();
//         StartCoroutine(DropSequence());
//     }

//     IEnumerator DropSequence()
//     {
//         // 1️⃣ الأرض تقع
//         floorCollider.enabled = false;

//         // 2️⃣ نستنى السقوط
//         yield return new WaitForSeconds(fallTime);

//         // 3️⃣ نرجع XR Origin لمكان البداية
//         xrOrigin.transform.position = respawnPoint.position;
//         xrOrigin.transform.rotation = respawnPoint.rotation;

//         // 4️⃣ Reset الكاميرا
//         xrOrigin.CameraYOffset = 0f;
//         xrOrigin.MoveCameraToWorldLocation(respawnPoint.position);

//         // 5️⃣ نرجّع الأرض
//         floorCollider.enabled = true;

//         triggered = false;
//     }
// }





using UnityEngine;
using Unity.XR.CoreUtils;
using System.Collections;

public class DropAndRespawn : MonoBehaviour
{
    public Collider floorCollider;
    public XROrigin xrOrigin;

    // ⭐ أكتر من مكان نزول
    public Transform[] respawnPoints;

    public float fallTime = 2f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.GetComponentInParent<XROrigin>() != null)
        {
            triggered = true;
            StartCoroutine(DropSequence());
        }
    }

    public void ForceRespawn()
    {
        if (triggered) return;

        triggered = true;
        StopAllCoroutines();
        StartCoroutine(DropSequence());
    }

    IEnumerator DropSequence()
    {
        // 1️⃣ الأرض تقع
        if (floorCollider != null)
            floorCollider.enabled = false;

        // 2️⃣ نستنى السقوط
        yield return new WaitForSeconds(fallTime);

        // 3️⃣ نختار مكان عشوائي
        int randomIndex = Random.Range(0, respawnPoints.Length);
        Transform point = respawnPoints[randomIndex];

        // 4️⃣ نرجّع اللاعب
        xrOrigin.transform.position = point.position;
        xrOrigin.transform.rotation = point.rotation;

        // 5️⃣ Reset الكاميرا
        xrOrigin.CameraYOffset = 0f;
        xrOrigin.MoveCameraToWorldLocation(point.position);

        // 6️⃣ نرجّع الأرض
        if (floorCollider != null)
            floorCollider.enabled = true;

        triggered = false;
    }
}
