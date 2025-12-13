using System;
using UnityEngine;

public class FreeSmurf : MonoBehaviour
{
    public GameObject Cage;
    public GameObject Key;
    public String smurfTag;
    private Animator smurfAnimator;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            Destroy(Cage);
            Destroy(Key);
            GameObject smurfDance = GameObject.FindWithTag(smurfTag);
            smurfAnimator = smurfDance.GetComponent<Animator>();
            smurfAnimator.SetBool("IsDancing", true);
        }
    }

}
