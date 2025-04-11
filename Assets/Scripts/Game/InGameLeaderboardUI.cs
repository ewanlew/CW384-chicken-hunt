using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class InGameLeaderboardUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI aboveText;
    [SerializeField] private TextMeshProUGUI currentText;
    [SerializeField] private TextMeshProUGUI belowText;

    public void UpdateDisplay(int playerScore) {
        List<ScoreEntry> sorted = LeaderboardManager.Instance.GetTopScores();
        int rank = LeaderboardManager.Instance.GetPlayerRank(playerScore) - 1;

        if (rank == 1 || sorted.Count == 0) {
            currentText.text = $"<b>{playerScore}</b> pts - Unranked";
            aboveText.text = "";
            belowText.text = "";
            return;
        }

        ScoreEntry current = sorted[rank]; 
        currentText.text = $"<b><color=#00FF00>#{rank + 1}</color></b> {current.score} pts";

        if (rank > 0) {
            ScoreEntry above = sorted[rank - 1];
            aboveText.text = $"<color=#888><size=90%>#{rank} {above.score} pts</size></color>";
        } else {
            aboveText.text = "";
        }

        if (rank < sorted.Count - 1) {
            ScoreEntry below = sorted[rank + 1];
            belowText.text = $"<color=#888><size=90%>#{rank + 2} {below.score} pts</size></color>";
        } else {
            belowText.text = "";
        }

    }
}
