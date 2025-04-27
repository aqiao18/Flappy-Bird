using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
    public List<int> topScores = new List<int>();

    public static void SaveLeaderboard(LeaderboardData leaderboardData)
    {
        string json = JsonUtility.ToJson(leaderboardData);
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    public static LeaderboardData LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("Leaderboard", "");
        if (string.IsNullOrEmpty(json))
        {
            return new LeaderboardData();
        }
        return JsonUtility.FromJson<LeaderboardData>(json);
    }

    public void AddScore(int score)
    {
        topScores.Add(score);
        topScores.Sort((a, b) => b.CompareTo(a)); // descending order
        if (topScores.Count > 10)
        {
            topScores = topScores.GetRange(0, 10);
        }
        SaveLeaderboard(this);
    }
}
