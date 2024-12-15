using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{

    public Image[] heartImages; // Array untuk gambar hati

    public void UpdateLives(int lives)
    {
        lives = Mathf.Clamp(lives, 0, heartImages.Length);

        for (int i =0; i< heartImages.Length; i++)
        {
            if(i < lives)
            {
                heartImages[i].color = Color.white;
            }
            else
            {
                heartImages[i].color = Color.black;
            }
        }
    }
}
