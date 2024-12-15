using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public Image[] lifeHearts;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;


    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (lives > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.black;
            }
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score + "m";
    }

}
