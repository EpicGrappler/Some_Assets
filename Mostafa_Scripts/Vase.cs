using UnityEngine;

public class Vase : MonoBehaviour
{
    private bool isBroken = false; // safety to avoid double calls
    public Canvas poster;
    public GameObject keyPrefab;
    void OnCollisionEnter(Collision collision)
    {
        if (isBroken) 
        return;

        if (collision.gameObject.CompareTag("Plane"))
        BreakVase();
    }

    void BreakVase()
    {
        isBroken = true;

        Debug.Log("VASE BROKEN → setting GameState");

        GameState.IsVaseDestroyed = true;
        if (!GameState.PosterHidden)
        {
            poster.gameObject.SetActive(false);
            GameState.PosterHidden = true;
        }
        Instantiate(keyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
