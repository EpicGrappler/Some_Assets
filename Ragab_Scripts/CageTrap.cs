// using UnityEngine;
// using UnityEngine.AI;

// public class CageTrap : MonoBehaviour
// {
//     private void OnCollisionEnter(Collision collision)
//     {
//         SharshabelRunning enemy =
//             collision.gameObject.GetComponentInParent<SharshabelRunning>();

//         if (enemy != null)
//         {
//             // وقف حركة العدو
//             enemy.enabled = false;

//             NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
//             if (agent != null)
//             {
//                 agent.isStopped = true;
//                 agent.enabled = false;
//             }

//             // دخّله جوه القفص
//             enemy.transform.position = transform.position;

//             // ثبّته
//             Rigidbody rb = enemy.GetComponent<Rigidbody>();
//             if (rb != null)
//             {
//                 rb.isKinematic = true;
//             }

//             Debug.Log("Enemy trapped");
//         }
//     }
// }

using UnityEngine;
using UnityEngine.AI;

public class CageTrap : MonoBehaviour
{
    public Transform enemySlot;

    private void OnCollisionEnter(Collision collision)
    {
        SharshabelRunning enemy =
            collision.gameObject.GetComponentInParent<SharshabelRunning>();

        if (enemy == null) return;

        TrapEnemy(enemy);
    }

    // void TrapEnemy(SharshabelRunning enemy)
    // {
    //     // 1️⃣ وقف سكربت العدو
    //     enemy.enabled = false;

    //     // 2️⃣ اقفل NavMeshAgent من غير isStopped
    //     NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
    //     if (agent != null && agent.enabled)
    //     {
    //         agent.enabled = false;
    //     }

    //     // 3️⃣ انقل العدو لمكان القفص
    //     if (enemySlot != null)
    //     {
    //         enemy.transform.position = enemySlot.position;
    //         enemy.transform.rotation =
    //             Quaternion.Euler(0, enemySlot.eulerAngles.y, 0);
    //     }

    //     // 4️⃣ ثبّت العدو
    //     Rigidbody rb = enemy.GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         rb.isKinematic = true;
    //         rb.linearVelocity = Vector3.zero;
    //         rb.angularVelocity = Vector3.zero;
    //     }

    //     // 5️⃣ خليه Child للقفص
    //     enemy.transform.SetParent(transform);

    //     Debug.Log("Enemy trapped correctly (NavMesh safe)");
    // }



    void TrapEnemy(SharshabelRunning enemy)
    {
        enemy.enabled = false;

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null && agent.enabled)
            agent.enabled = false;

        enemy.transform.position = enemySlot.position;
        enemy.transform.rotation =
            Quaternion.Euler(0, enemySlot.eulerAngles.y, 0);

        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        if (enemyRb != null)
        {
            enemyRb.isKinematic = true;
            enemyRb.linearVelocity = Vector3.zero;
            enemyRb.angularVelocity = Vector3.zero;
        }

        // ⭐ ثبّت القفص نفسه
        Rigidbody cageRb = GetComponent<Rigidbody>();
        if (cageRb != null)
        {
            cageRb.linearVelocity = Vector3.zero;
            cageRb.angularVelocity = Vector3.zero;
            cageRb.isKinematic = true;
        }

        enemy.transform.SetParent(transform);
    }

}

