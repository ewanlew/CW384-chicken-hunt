using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance; // global reference

    private const string SaveKey = "LeaderboardData"; // key for saving
    private List<ScoreEntry> leaderboard = new List<ScoreEntry>(); // list of scores

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // stay between scenes
            LoadLeaderboard(); // try load previous scores
        } else {
            Destroy(gameObject); // prevent duplicates
        }
    }

    public void AddScore(int score) {
        leaderboard.Add(new ScoreEntry(score)); // add new score
        leaderboard = leaderboard
            .OrderByDescending(e => e.score) // sort highest to lowest
            .Take(100) // keep only top 100
            .ToList();

        SaveLeaderboard(); // store updated list
    }

    public List<ScoreEntry> GetTopScores() {
        return leaderboard; // return full list
    }

    public int GetPlayerRank(int score) {
        var sorted = leaderboard.OrderByDescending(e => e.score).ToList(); // sorted copy

        for (int i = 0; i < sorted.Count; i++) {
            if (score >= sorted[i].score) {
                return i + 1; // return rank (1-indexed)
            }
        }

        return sorted.Count + 1; // lower than all existing
    }

    private void SaveLeaderboard() {
        string json = JsonUtility.ToJson(new LeaderboardData(leaderboard)); // serialise
        PlayerPrefs.SetString(SaveKey, json); // save to prefs
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard() {
        if (PlayerPrefs.HasKey(SaveKey)) {
            string json = PlayerPrefs.GetString(SaveKey); // load string
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json); // deserialise
            leaderboard = data.entries; // load list
        } else {
            leaderboard = new List<ScoreEntry>(); // start fresh
        }
    }

    public void ResetLeaderboard() {
        leaderboard.Clear(); // clear list
        PlayerPrefs.DeleteKey(SaveKey); // wipe key
        PlayerPrefs.Save(); // apply
    }
}

[System.Serializable]
public class LeaderboardData {
    public List<ScoreEntry> entries; // scores container

    public LeaderboardData(List<ScoreEntry> entries) {
        this.entries = entries;
    }
}
