using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recentEntryText; // recent player score display
    [SerializeField] private GameObject entryPrefab; // template for each score row
    [SerializeField] private Transform contentParent; // where rows get added
    [SerializeField] private Button backButton; // return to menu
    [SerializeField] private Button resetButton; // wipe scores

    void Start() {
        // hook up buttons
        backButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        resetButton.onClick.AddListener(() =>  {
            LeaderboardManager.Instance.ResetLeaderboard();
            PopulateLeaderboard(); // refresh display
        });

        // wait a tick if manager isn’t ready
        if (LeaderboardManager.Instance == null) {
            Debug.LogWarning("LeaderboardManager not ready. Retrying...");
            Invoke(nameof(PopulateLeaderboard), 0.2f); // try again shortly
        } else {
            PopulateLeaderboard();
        }
    }

    void PopulateLeaderboard() {
        List<ScoreEntry> scores = LeaderboardManager.Instance.GetTopScores(); // all scores

        // load most recent score entry from PlayerPrefs
        string recentJson = PlayerPrefs.GetString("RecentScore", "");
        ScoreEntry recent = null;

        if (!string.IsNullOrEmpty(recentJson)) {
            recent = JsonUtility.FromJson<ScoreEntry>(recentJson);
        } else if (scores.Count > 0) {
            recent = scores[0]; // fallback to best score if none saved
        }

        if (recent != null) {
            int rank = LeaderboardManager.Instance.GetPlayerRank(recent.score);
            recentEntryText.text =
                $"<b><size=150%>#{rank}</size></b> " +
                $"<size=110%>{recent.score} pts</size> " +
                $"<color=#555555><size=90%>{recent.date} {recent.time}</size></color>";
        } else {
            recentEntryText.text = "No scores yet. <b>Play now!</b>";
        }

        int pos = 1;
        foreach (ScoreEntry entry in scores) {
            GameObject go = Instantiate(entryPrefab, contentParent); // create entry
            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();

            // pick colour based on position
            string color = "#000000"; // default black
            if (pos == 1) color = "#edb200"; // gold
            else if (pos == 2) color = "#9c9c9c"; // silver
            else if (pos == 3) color = "#9c5e21"; // bronze

            text.text =
                $"<color={color}><b><size=150%>#{pos}</size></b> " +
                $"<size=110%>{entry.score} pts</size></color> " +
                $"<color=#555555><size=90%>{entry.date} {entry.time}</size></color>";

            pos++;
        }
    }
}
