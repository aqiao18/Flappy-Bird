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
    [SerializeField] private Button clearScoreButton;
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

    #region Unity Callbacks
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        HideAllMenus();
        if (!IsRestarting)
        {
            mainMenuPanel.SetActive(true);
            SoundManager.Instance.PlayThemeMusic();
        }
        else
        {
            mainMenuPanel.SetActive(false);
            SoundManager.Instance.StopThemeMusic();
            HideScrollingBackground();
            IsRestarting = false;
        }
        Time.timeScale = 0f;

        restartButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(GoHome);
        playButton.onClick.AddListener(StartGame);
        hiScoreButton.onClick.AddListener(ShowHighScore);
        leaderboardBackButton.onClick.AddListener(HideHighScore);
        clearScoreButton.onClick.AddListener(ClearScores);
    }
    #endregion

    #region Button Methods
    private void RestartGame()
    {
        IsRestarting = true;
        ResetAll(false);
    }

    private void StartGame()
    {
        mainMenuPanel.SetActive(false);
        SoundManager.Instance.StopThemeMusic();
        HideScrollingBackground();
        currentScore.ShowScore();
        BackgroundLooper.pauseScrolling = true;
    }

    private void GoHome()
    {
        IsRestarting = false;
        ResetAll(goingHome: true);
        SoundManager.Instance.PlayThemeMusic();
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

    private void ClearScores()
    {
        PlayerPrefs.DeleteKey("Leaderboard");
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        Debug.Log("Clearing all score data");
    }
    #endregion

    #region Game Management
    public void ShowGameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);

        ScoreManager.Instance.CheckHighScore(finalScore);
        BackgroundLooper.pauseScrolling = true;
        int highScore = ScoreManager.Instance.GetHighScore();

        LeaderboardData data = LeaderboardData.LoadLeaderboard();
        data.AddScore(finalScore);

        scoreText.text = "Score: " + finalScore;
        hiScoreText.text = "Best: " + highScore;

        Time.timeScale = 0f;
    }

    public void MarkGameStarted()
    {
        GameStarted = true;
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }

    private void ResetAll(bool goingHome)
    {
        mainMenuPanel.SetActive(goingHome ? true : false);

        gameOverPanel.SetActive(false);
        
        flappyBird.ResetPlayer();
        
        pipes.resetPipes();
        
        currentScore.ResetScore();
        if (!goingHome) currentScore.ShowScore();
        GameStarted = false;
        if (goingHome) BackgroundLooper.pauseScrolling = false;
    }
    #endregion

    #region Helpers
    private void HideAllMenus()
    {
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        SoundManager.Instance.StopThemeMusic();
    }
    public bool IsMainMenuOpen()
    {
        return mainMenuPanel.activeSelf;
    }
    private void HideScrollingBackground() {
        if (scrollingBackground != null) scrollingBackground.gameObject.SetActive(false);
    }

    private void ShowScrollingBackground() {
        if (scrollingBackground != null) scrollingBackground.gameObject.SetActive(true);
    }
    #endregion
}