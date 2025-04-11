using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class InGameLeaderboardUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI aboveText;
    [SerializeField] private TextMeshProUGUI currentText;

    void Start() {
        // start w 0 pts on lb
        currentText.text = "<size=110%>0 points</size>";
        
        List<ScoreEntry> topScores = LeaderboardManager.Instance.GetTopScores();
        if (topScores.Count > 0) {
            ScoreEntry lowest = topScores[topScores.Count - 1];
            aboveText.text =
                $"<b><size=90%>#{topScores.Count}</size></b> " +
                $"<size=100%>{lowest.score} points</size> ";
        } else {
            aboveText.text = "<i>No scores yet</i>";
        }
    }


    public void UpdateDisplay(int score) {
        List<ScoreEntry> sorted = LeaderboardManager.Instance.GetTopScores();
        Debug.Log($"Updating compact leaderboard for score: {score}");

        int rank = LeaderboardManager.Instance.GetPlayerRank(score) - 1;

        if (rank == -1 || sorted.Count == 0) {
            currentText.text = $"<b>{score}</b> points - Unranked";
            aboveText.text = "";
            return;
        }

        ScoreEntry current = sorted[rank]; 
        currentText.text = $"<b><color=#00FF00>#{rank + 1}</color></b> {current.score} points";

        if (rank > 0) {
            ScoreEntry above = sorted[rank - 1];
            aboveText.text = $"<color=#888><size=90%>#{rank} {above.score} points</size></color>";
        } else {
            aboveText.text = "";
        }
    }
}
