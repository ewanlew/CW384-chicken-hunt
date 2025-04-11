using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI recentEntryText;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private Button backButton;
    [SerializeField] private Button resetButton;

    void Start() {
        backButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        resetButton.onClick.AddListener(() =>  {
            LeaderboardManager.Instance.ResetLeaderboard();
            PopulateLeaderboard();
        });

        if (LeaderboardManager.Instance == null) {
            Debug.LogWarning("LeaderboardManager not ready. Retrying...");
            Invoke(nameof(PopulateLeaderboard), 0.2f); // try again shortly
        } else {
            PopulateLeaderboard();
        }
    }

    void PopulateLeaderboard() {
        List<ScoreEntry> scores = LeaderboardManager.Instance.GetTopScores();

        string recentJson = PlayerPrefs.GetString("RecentScore", "");
        ScoreEntry recent = null;

        if (!string.IsNullOrEmpty(recentJson)) {
            recent = JsonUtility.FromJson<ScoreEntry>(recentJson);
        } else if (scores.Count > 0) {
            recent = scores[0]; // fallback to top score if no recent
        }

        if (recent != null) {
            int rank = LeaderboardManager.Instance.GetPlayerRank(recent.score);

            recentEntryText.text =
                $"<b><size=150%>#{rank}</size></b> " +
                $"<size=110%>{recent.score} pts</size> " +
                $"<color=#AAAAAA><size=90%>{recent.date} {recent.time}</size></color>";
        } else {
            recentEntryText.text = "No scores yet. <b>Play now!</b>";
        }

        int pos = 1;
        foreach (ScoreEntry entry in scores) {
            GameObject go = Instantiate(entryPrefab, contentParent);
            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();

            text.text =
                $"<b><size=150%>#{pos}</size></b> " +
                $"<size=110%>{entry.score} pts</size> " +
                $"<color=#AAAAAA><size=90%>{entry.date} {entry.time}</size></color>";

            pos++;
        }
    }
}
