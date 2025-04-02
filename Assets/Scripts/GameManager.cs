using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            UpdateUI();
        } else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amt){
        score += amt;
        UpdateUI();
    }

    public void Miss() {
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
    }
}
