using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    private int score;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("ScoreText not assigned in the inspector.");
        }
    }

    public void SaveScore()
    {
        string filePath = Application.persistentDataPath + "/score.csv";
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            string playerName = PlayerNameInput.playerName;
            string dateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            writer.WriteLine($"{playerName},{score},{dateTime}");
        }
        Debug.Log("Score saved to " + filePath);
    }

    public void DisplayScores()
    {
        string filePath = Application.persistentDataPath + "/score.csv";
        if (File.Exists(filePath))
        {
            string[] scores = File.ReadAllLines(filePath);
            foreach (string scoreEntry in scores)
            {
                Debug.Log(scoreEntry); // Ou affichez-le dans un UI Text/ TMP Text si vous avez un UI dans la scène des scores
            }
        }
    }
}
