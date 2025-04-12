using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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
        UpdateUI();
        gameOverPanel.SetActive(false);
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amt){
        if (isDoublePointsActive) { amt *= 2; }
        score += amt;
        
        UpdateUI();
        leaderboardUI?.UpdateDisplay(score);
    }

    public void Miss(bool nearMiss = false) {
        if (nearMiss) {
            lives -= (float) 0.5;
        } else {
            lives -= (float) 1;
        }

        if (lives <= 0) {
            GameOver();
        }

        UpdateUI();
    }

    public void TriggerTypingChallnge() {
        TimeManager.FreezeTime();

        PlayerShooter shooter = Object.FindFirstObjectByType<PlayerShooter>();
        if (shooter != null) {
            shooter.enabled = false;
        }

        typingPanel.SetActive(true);

        ItemType reward = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);

        typingChallenge.SetRewardItem(reward);
        typingChallenge.BeginNewPrompt();
    }

    public void SlowTimeEffect() {
        StartCoroutine(SlowTimeCoroutine());
    }

    public IEnumerator SlowTimeCoroutine() {
        TimeManager.SetSlowTime(0.5f);

        float duration = 15f;
        float elapsed = 0f;

        while (elapsed < duration) {
            if (!PauseState.IsGamePaused && !TypingState.IsUserTyping) {
                elapsed += Time.unscaledDeltaTime;
            }
            yield return null;
        }

        if (!TimeManager.IsTimeFrozen) {
            TimeManager.SetNormalSpeed();
        }
    }


    public void AddLife(float amt) {
        lives += amt;
        UpdateUI();
    }

    public void DoublePointsEffect() {
        StartCoroutine(DoublePointsCoroutine());
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

        isDoublePointsActive = false;
    }


    void UpdateUI()
    {
        livesText.text = "x" + lives.ToString("0.0");
    }

    void GameOver() {
        TimeManager.SetSlowTime(0.5f);
        LeaderboardManager.Instance.AddScore(score);

        ScoreEntry recent = new ScoreEntry(score);
        string json = JsonUtility.ToJson(recent);
        PlayerPrefs.SetString("RecentScore", json);
        PlayerPrefs.Save();


        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
    }

    public void Replay() {
        TimeManager.SetNormalSpeed();

        if (typingPanel != null) {
            typingPanel.SetActive(false);
        }

        if (typingChallenge != null) {
            typingChallenge.ForceCancelChallenge();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu() {
        TimeManager.SetNormalSpeed();
        SceneManager.LoadScene(0);
    }
}
