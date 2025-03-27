using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static int currentScore { get; private set; }
    public TextMeshProUGUI scoreText; // UI untuk menampilkan skor

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    public static void AddScore(int amount)
    {
        currentScore += amount;
        
        if (Instance != null) 
        {
            Instance.UpdateScoreUI();
        }
    }


    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
