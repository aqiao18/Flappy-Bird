using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set;}

    public TMP_Text scoreText;
    private int score = 0;
    const string HighScoreKey = "HighScore";

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void HideScore()
    {
        scoreText.gameObject.SetActive(false);
    }
    
    public void ShowScore()
    {
        scoreText.gameObject.SetActive(true);
    }

    public void CheckHighScore(int scoreToCheck)
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (scoreToCheck > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, scoreToCheck);
            PlayerPrefs.Save();
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void ResetScore() {
        score = 0;
        scoreText.text = score.ToString();
    }
}
