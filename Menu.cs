using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartRun()
    {
        int selectedCharacterIndex = 0; // Tentukan indeks karakter yang dipilih (misalnya 0, 1, 2, dst.)
        GameManager.gm.StartRun(selectedCharacterIndex);    
    }
}
