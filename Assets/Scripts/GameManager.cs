using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver { get; private set; }

    private void Awake()
    {
        isGameOver = false;
        Time.timeScale = 1;
    }

    public static void GameOver()
    {
        isGameOver = true;
    }

    public static void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void backtoMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}