using UnityEngine;

public class CollisionDetect : MonoBehaviour
{

    [SerializeField] private GameObject thePlayer;   // Referensi ke objek pemain
    [SerializeField] private GameObject playerAnim;  // Referensi ke animator pemain

    void OnTriggerEnter(Collider other)
    {
            thePlayer.GetComponent<PlayerMovement>().enabled = false;
            
    }
}
