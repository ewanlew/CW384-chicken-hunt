using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // singleton for easy access
    [SerializeField] private TypingChallenge typingChallenge;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] public GameObject typingPanel;

    [Header("Text Objects")] 
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Init Values")] 
    [SerializeField] private int score = 0;
    [SerializeField] private float lives = 2f;
    [SerializeField] private bool isDoublePointsActive = false;

    public InGameLeaderboardUI leaderboardUI;

    void Start() {
        UpdateUI(); // update score + lives display
        gameOverPanel.SetActive(false); // hide game over screen at start
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject); // make sure only one exists
        }
    }

    public void AddScore(int amt){
        if (isDoublePointsActive) { amt *= 2; } // double points if active
        score += amt;

        UpdateUI();
        leaderboardUI?.UpdateDisplay(score); // update in-game leaderboard
    }

    public void Miss(bool nearMiss = false) {
        if (nearMiss) {
            lives -= (float) 0.5;
        } else {
            lives -= (float) 1;
        }

        if (lives <= 0) {
            GameOver(); // trigger game over if out of lives
        }

        UpdateUI(); // reflect on UI
    }

    public void TriggerTypingChallnge() {
        TimeManager.FreezeTime(); // pause game while typing

        PlayerShooter shooter = Object.FindFirstObjectByType<PlayerShooter>();
        if (shooter != null) {
            shooter.enabled = false; // stop shooting
        }

        typingPanel.SetActive(true); // show panel

        ItemType reward = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);

        typingChallenge.SetRewardItem(reward); // pick reward
        typingChallenge.BeginNewPrompt(); // start challenge
    }

    public void SlowTimeEffect() {
        StartCoroutine(SlowTimeCoroutine()); // slow motion item logic
    }

    public IEnumerator SlowTimeCoroutine() {
        TimeManager.SetSlowTime(0.5f); // reduce timescale

        float duration = 15f;
        float elapsed = 0f;

        // only count time if not paused or typing
        while (elapsed < duration) {
            if (!PauseState.IsGamePaused && !TypingState.IsUserTyping) {
                elapsed += Time.unscaledDeltaTime;
            }
            yield return null;
        }

        if (!TimeManager.IsTimeFrozen) {
            TimeManager.SetNormalSpeed(); // reset timescale
        }
    }

    public void AddLife(float amt) {
        lives += amt;
        UpdateUI(); // update HUD
    }

    public void DoublePointsEffect() {
        StartCoroutine(DoublePointsCoroutine()); // temp double score boost
    }

    public IEnumerator DoublePointsCoroutine() {
        isDoublePointsActive = true;

        float duration = 10f;
        float elapsed = 0f;

        while (elapsed < duration) {
            if (!PauseState.IsGamePaused && !TypingState.IsUserTyping) {
                elapsed += Time.unscaledDeltaTime;
            }
            yield return null;
        }

        isDoublePointsActive = false; // reset
    }

    void UpdateUI()
    {
        livesText.text = "x" + lives.ToString("0.0"); // always one decimal place
    }

    void GameOver() {
        TimeManager.FreezeTime(); // stop everything
        AudioManager.Instance.PlaySFX(AudioManager.Instance.loseGame); // play fail sound
        LeaderboardManager.Instance.AddScore(score); // submit score

        // save this run’s score locally
        ScoreEntry recent = new ScoreEntry(score);
        string json = JsonUtility.ToJson(recent);
        PlayerPrefs.SetString("RecentScore", json);
        PlayerPrefs.Save();

        gameOverPanel.SetActive(true); // show game over screen
        finalScoreText.text = "Final Score: " + score; // display final score
    }

    public void Replay() {
        TimeManager.SetNormalSpeed(); // restore speed

        if (typingPanel != null) {
            typingPanel.SetActive(false); // hide typing
        }

        if (typingChallenge != null) {
            typingChallenge.ForceCancelChallenge(); // cancel current prompt
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
    }

    public void ReturnToMenu() {
        TimeManager.SetNormalSpeed(); // just in case
        SceneManager.LoadScene(0); // go back to menu
    }
}
