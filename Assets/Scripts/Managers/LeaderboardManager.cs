using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private const string SaveKey = "LeaderboardData";
    private List<ScoreEntry> leaderboard = new List<ScoreEntry>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
        } else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score) {
        leaderboard.Add(new ScoreEntry(score));
        leaderboard = leaderboard.OrderByDescending(e => e.score).Take(100).ToList(); // keep top 100
        SaveLeaderboard();
    }

    public List<ScoreEntry> GetTopScores() {
        return leaderboard;
    }

    public int GetPlayerRank(int score) {
        var sorted = leaderboard.OrderByDescending(e => e.score).ToList();
        for (int i = 0; i < sorted.Count; i++) {
            if (score >= sorted[i].score) {
                return i + 1;
            }
        }
        return sorted.Count + 1;
    }

    private void SaveLeaderboard() {
        string json = JsonUtility.ToJson(new LeaderboardData(leaderboard));
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard() {
        if (PlayerPrefs.HasKey(SaveKey)) {
            string json = PlayerPrefs.GetString(SaveKey);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            leaderboard = data.entries;
        } else {
            leaderboard = new List<ScoreEntry>();
        }
    }
}

[System.Serializable]
public class LeaderboardData {
    public List<ScoreEntry> entries;

    public LeaderboardData(List<ScoreEntry> entries) {
        this.entries = entries;
    }
}
