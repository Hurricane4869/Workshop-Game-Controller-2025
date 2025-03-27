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
    void Start()
    {
        SoundManager.PlayBGM(BGMType.GAMEPLAY);
    }

    public static void GameOver()
    {
        isGameOver = true;
        SoundManager.StopBGM();
    }

    public static void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void backtoMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        SoundManager.StopBGM();
    }
}