using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] AudioSource coinFX;

    void OnTriggerEnter(Collider other)
    {
        coinFX.Play();  // jika trigger on akan mem-Play sound coin 

        MasterInfo.coinCount += 1;  // jika trigger on Menambah jumlah coin +1

        this.gameObject.SetActive(false); // coin akan hilang jika trigger on
    }
}