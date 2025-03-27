using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image playerHealthBar;
    public float playerHealthBarFullX = 492;

    public GameObject gameOverUI;

    public PlayerController player;

    void Update()
    {
        if (scoreText)
        {
            scoreText.text = ScoreManager.currentScore.ToString();
        }

        if (playerHealthBar)
        {
            Vector2 size = playerHealthBar.rectTransform.sizeDelta;
            size.x = player.health / player.healthMax * playerHealthBarFullX;

            playerHealthBar.rectTransform.sizeDelta = size;
        }

        if (gameOverUI && GameManager.isGameOver)
        {
            gameOverUI.SetActive(GameManager.isGameOver);
            Time.timeScale = 0;
        }
        if (gameOverUI && !GameManager.isGameOver)
        {
            gameOverUI.SetActive(GameManager.isGameOver);
        }
    }
}