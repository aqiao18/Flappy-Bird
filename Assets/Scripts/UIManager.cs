using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public bool GameStarted { get; private set; } = false;
    [SerializeField] private FlappyBird flappyBird;
    [SerializeField] private PipeManager pipes;
    [SerializeField] private ScoreManager currentScore;
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI hiScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    [Header("Main Menu UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button hiScoreButton;

    [Header("Leaderboard UI")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private List<TMPro.TextMeshProUGUI> leaderboardEntries;
    [SerializeField] private Button leaderboardBackButton;

    [Header("MainMenu Scrolling Background")]
    [SerializeField] private RawImage scrollingBackground;
    private static bool IsRestarting = false;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        // PlayerPrefs.DeleteKey("Leaderboard");
        // PlayerPrefs.DeleteKey("HighScore");
        // PlayerPrefs.Save();
    }

    private void Start()
    {
        HideAllMenus();
        if (!IsRestarting)
        {
            mainMenuPanel.SetActive(true);
        }
        else
        {
            mainMenuPanel.SetActive(false);
            HideScrollingBackground();
            IsRestarting = false;
        }
        Time.timeScale = 0f;

        restartButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(GoHome);
        playButton.onClick.AddListener(StartGame);
        hiScoreButton.onClick.AddListener(ShowHighScore);
        leaderboardBackButton.onClick.AddListener(HideHighScore);
    }

    private void RestartGame()
    {
        IsRestarting = true;
        resetAll(false);
    }

    private void StartGame()
    {
        mainMenuPanel.SetActive(false);
        HideScrollingBackground();
        currentScore.ShowScore();
    }

    public void ShowGameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);

        ScoreManager.Instance.CheckHighScore(finalScore);
        int highScore = ScoreManager.Instance.GetHighScore();

        LeaderboardData data = LeaderboardData.LoadLeaderboard();
        data.AddScore(finalScore);

        scoreText.text = "Score: " + finalScore;
        hiScoreText.text = "Best: " + highScore;

        Time.timeScale = 0f;
    }

    public void ShowHighScore()
    {
        leaderboardPanel.SetActive(true);

        LeaderboardData data = LeaderboardData.LoadLeaderboard();

        int scoreIndex = 0;

        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            if (scoreIndex < data.topScores.Count && data.topScores[scoreIndex] > 0)
            {
                leaderboardEntries[i].text = $"{i + 1}. {data.topScores[scoreIndex]}";
                scoreIndex++;
            }
            else
            {
                leaderboardEntries[i].text = $"{i + 1}. ---";
            }
        }
    }

    public void HideHighScore()
    {
        leaderboardPanel.SetActive(false);
    }

    public void MarkGameStarted()
    {
        GameStarted = true;
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }

    private void HideAllMenus()
    {
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }
    public bool IsMainMenuOpen()
    {
        return mainMenuPanel.activeSelf;
    }
    private void GoHome()
    {
        IsRestarting = false;
        resetAll(goingHome: true);
    }
    private void HideScrollingBackground() {
        if (scrollingBackground != null) scrollingBackground.gameObject.SetActive(false);
    }

    private void ShowScrollingBackground() {
        if (scrollingBackground != null) scrollingBackground.gameObject.SetActive(true);
    }

    private void resetAll(bool goingHome) {
        mainMenuPanel.SetActive(goingHome ? true : false);

        gameOverPanel.SetActive(false);
        
        flappyBird.ResetPlayer();
        
        pipes.resetPipes();
        
        currentScore.ResetScore();
        if (!goingHome) currentScore.ShowScore();
        GameStarted = false;
    }
}