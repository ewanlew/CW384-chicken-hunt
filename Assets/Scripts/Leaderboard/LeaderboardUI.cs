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

    void Start()
    {
        PopulateLeaderboard();
        backButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
    }

    void PopulateLeaderboard() {
        List<ScoreEntry> scores = LeaderboardManager.Instance.GetTopScores();

        if (scores.Count > 0) {
            var latest = scores[0];
            int rank = LeaderboardManager.Instance.GetPlayerRank(latest.score);

            recentEntryText.text =
                $"<b><size=150%>#{rank}</size></b> " +
                $"<size=110%>{latest.score} pts</size> " +
                $"<color=#AAAAAA><size=90%>{latest.date} {latest.time}</size></color>";
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
