using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InGameLeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject abovePanel; // background panel for the 'above' score
    [SerializeField] private TextMeshProUGUI aboveText; // score right above the player
    [SerializeField] private TextMeshProUGUI currentText; // player’s current rank + score

    private int previousRank = -1; // store last known rank to detect changes

    void Start() {
        // start with 0 score shown
        currentText.text = "<size=110%>0 points</size>";
        
        List<ScoreEntry> topScores = LeaderboardManager.Instance.GetTopScores();
        if (topScores.Count > 0) {
            ScoreEntry lowest = topScores[topScores.Count - 1];
            aboveText.text =
                $"<b><size=90%>#{topScores.Count}</size></b> " +
                $"<size=100%>{lowest.score} points</size> ";
        } else {
            aboveText.text = "<i>No scores yet</i>"; // fallback if list is empty
        }
    }

    public void UpdateDisplay(int score) {
        List<ScoreEntry> sorted = LeaderboardManager.Instance.GetTopScores();
        int rank = LeaderboardManager.Instance.GetPlayerRank(score); // get current rank

        if (rank <= 0) {
            // unranked state (shouldn’t really happen)
            currentText.text = $"<b>#–</b>  {score} points";
            aboveText.text = "";
            if (abovePanel != null) { 
                abovePanel.SetActive(false);
                previousRank = -1;
                return;
            }
        }

        // play sound if player’s rank has improved
        if (previousRank > 0 && rank < previousRank) {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.increaseLeaderboard);
        }

        previousRank = rank;

        // update rank and score
        currentText.text = $"<b>#{rank}</b>  {score} points";

        // update score directly above the player (unless they’re in first place)
        if (rank > 1 && rank - 2 < sorted.Count) {
            ScoreEntry aboveEntry = sorted[rank - 2];
            aboveText.text = $"#{rank - 1}  {aboveEntry.score} points";
            if (abovePanel != null) abovePanel.SetActive(true);
        } else {
            // nothing above to show
            aboveText.text = "";
            if (abovePanel != null) abovePanel.SetActive(false);
        }
    }
}
