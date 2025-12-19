using System;
using UnityEngine;

public class Free2 : MonoBehaviour
{
    public GameObject Cage;
    public GameObject Key;
    public String smurfTag;
    private Animator smurfAnimator;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key2"))
        {
            Destroy(Cage);
            Destroy(Key);
            GameObject smurfDance = GameObject.FindWithTag(smurfTag);
            smurfAnimator = smurfDance.GetComponent<Animator>();
            smurfAnimator.SetBool("IsDancing", true);
        }

        else if (other.CompareTag("Tarek_Key"))
        {
            Destroy(Cage);
            Destroy(Key);
            GameObject smurfDance = GameObject.FindWithTag(smurfTag);
            smurfAnimator = smurfDance.GetComponent<Animator>();
            smurfAnimator.SetBool("IsDancing", true);
            GameState.IsKeyCollided_Tarek = true;
        }
    }

}
