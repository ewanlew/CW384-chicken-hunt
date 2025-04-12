using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypingChallenge : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI inputText;

    private ItemType pendingReward;
    private string currentWord = "";
    private int currentIndex = 0;
    private bool challengeActive = false;

    private string[] possibleWords = { "chicken", "poultry", "feathers", "bird", "cluck", "hen", "fried", "kentucky", "nugget", "wings", "fowl", "breast", "beak", "eggs" };

    public void BeginNewPrompt() {
        currentWord = possibleWords[Random.Range(0, possibleWords.Length)];
        promptText.text = $"Type \"{currentWord}\"";
        inputText.text = "";
        currentIndex = 0;
        challengeActive = true;
        TypingState.IsUserTyping = true;
    }

    void Update() {
        if (!challengeActive || Time.timeScale > 0f) { return; }

        foreach (char c in Input.inputString) { 
            if (currentIndex >= currentWord.Length) { return; }

            if (char.ToLower(c) == currentWord[currentIndex]) {
                inputText.text += c;
                currentIndex++;

                if (currentIndex == currentWord.Length) {
                    ChallengeSuccess();
                }
            } else {
                StartCoroutine(ChallengeFail());
            }
        }
    }

    void ChallengeSuccess() {
        challengeActive = false;
        TypingState.IsUserTyping = false;
        ItemManager.Instance.AddItem(pendingReward);

        EndChallenge();
    }

    IEnumerator ChallengeFail() {
        inputText.text = "<b><color=red>WRONG!</color></b>";
        challengeActive = false;
        TypingState.IsUserTyping = false;
        yield return new WaitForSecondsRealtime(1f);
        EndChallenge();
    }

    void EndChallenge() {
        TimeManager.ResumeTime();
        GameManager.Instance.typingPanel.SetActive(false);

        // cleanup any leftover golden chickens
        Chicken[] chickens = Object.FindObjectsByType<Chicken>(FindObjectsSortMode.None);
        foreach (var chick in chickens) {
            if (chick != null && chick.isGolden && chick.isHidden) {
                Destroy(chick.gameObject);
            }
        }

        PlayerShooter shooter = Object.FindFirstObjectByType<PlayerShooter>();
        if (shooter != null) {
            shooter.enabled = true;
        }
    }

    public void ForceCancelChallenge() {
        StopAllCoroutines();
        GameManager.Instance.typingPanel.SetActive(false);
        challengeActive = false;
        TypingState.IsUserTyping = false;
        currentIndex = 0;

        PlayerShooter shooter = Object.FindFirstObjectByType<PlayerShooter>();
        if (shooter != null) {
            shooter.enabled = true;
        }

        TimeManager.ResumeTime();
    }


    public void SetRewardItem(ItemType reward) {
        pendingReward = reward;
    }
}
