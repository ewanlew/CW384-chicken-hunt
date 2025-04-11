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
        int rank = LeaderboardManager.Instance.GetPlayerRank(score);

        if (rank <= 0) {
            currentText.text = $"<b>#–</b>  {score} pts";
            aboveText.text = "";
            return;
        }

        // show current player
        currentText.text = $"<b>#{rank}</b>  {score} pts";

        // show player above (only if not rank 1)
        if (rank > 1 && rank - 2 < sorted.Count) {
            ScoreEntry aboveEntry = sorted[rank - 2];
            aboveText.text = $"#{rank - 1}  {aboveEntry.score} pts";
        } else {
            aboveText.text = "";
        }
    }
}
