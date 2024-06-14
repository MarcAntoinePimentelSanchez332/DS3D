using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class HighScoresManager : MonoBehaviour
{
    public TextMeshProUGUI highScoresText;

    void Start()
    {
        DisplayHighScores();
    }

    void DisplayHighScores()
    {
        string filePath = Application.persistentDataPath + "/score.csv";
        if (File.Exists(filePath))
        {
            string[] scores = File.ReadAllLines(filePath);
            List<(string playerName, int score, string dateTime)> scoreList = new List<(string playerName, int score, string dateTime)>();

            foreach (string score in scores)
            {
                string[] split = score.Split(',');
                if (split.Length == 3 && int.TryParse(split[1], out int parsedScore))
                {
                    scoreList.Add((split[0], parsedScore, split[2]));
                }
            }

            scoreList.Sort((a, b) => b.score.CompareTo(a.score)); // Tri décroissant des scores

            highScoresText.text = "High Scores:\n";
            for (int i = 0; i < Mathf.Min(10, scoreList.Count); i++) // Afficher les 10 meilleurs scores
            {
                highScoresText.text += $"{i + 1}. {scoreList[i].playerName}: {scoreList[i].score} ({scoreList[i].dateTime})\n";
            }
        }
        else
        {
            highScoresText.text = "No high scores yet.";
        }
    }
}
