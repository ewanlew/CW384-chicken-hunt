using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    public float lives = 2f;
    public TextMeshProUGUI livesText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

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
        score += amt;
        UpdateUI();
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

    void UpdateUI()
    {
        livesText.text = "Lives: " + lives.ToString("0.0");
        scoreText.text = "Score: " + score;
    }

    void GameOver() {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
    }

    public void Replay() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ReturnToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
