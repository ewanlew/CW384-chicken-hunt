using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypingChallenge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText; // shows the target word
    [SerializeField] private TextMeshProUGUI inputText; // shows what player types

    private ItemType pendingReward; // reward to give on success
    private string currentWord = ""; // word to type
    private int currentIndex = 0; // how far through the word
    private bool challengeActive = false; // if we're currently in a challenge

    // chkciekn-related words to randomly choose from
    private string[] possibleWords = { "chicken", "poultry", "feathers", "bird", "cluck", "hen", "fried", "kentucky", "nugget", "wings", "fowl", "breast", "beak", "eggs", "tennessee", "batter", "rooster", "chick", "protein" };

    public void BeginNewPrompt() {
        currentWord = possibleWords[Random.Range(0, possibleWords.Length)]; // pick a random word
        promptText.text = $"Type \"{currentWord}\""; // show the word
        inputText.text = ""; // clear input
        currentIndex = 0;
        challengeActive = true;
        TypingState.IsUserTyping = true; // tell game we're typing
    }

    void Update() {
        if (!challengeActive || Time.timeScale > 0f) { return; } // only check input while paused

        foreach (char c in Input.inputString) { 
            if (currentIndex >= currentWord.Length) { return; } // finished typing

            if (char.ToLower(c) == currentWord[currentIndex]) {
                inputText.text += c; // add to display
                currentIndex++;

                if (currentIndex == currentWord.Length) {
                    ChallengeSuccess(); // word completed
                }
            } else {
                StartCoroutine(ChallengeFail()); // wrong key
            }
        }
    }

    void ChallengeSuccess() {
        challengeActive = false;
        TypingState.IsUserTyping = false;
        ItemManager.Instance.AddItem(pendingReward); // give item

        EndChallenge(); // clean up
    }

    IEnumerator ChallengeFail() {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.typingChallengeFail); // fail sound
        inputText.text = "<b><color=red>WRONG!</color></b>"; // show error
        challengeActive = false;
        TypingState.IsUserTyping = false;
        yield return new WaitForSecondsRealtime(1f); // pause to show message
        EndChallenge();
    }

    void EndChallenge() {
        TimeManager.ResumeTime(); // resume gameplay
        GameManager.Instance.typingPanel.SetActive(false); // hide panel

        // destroy any hidden golden chickens
        Chicken[] chickens = Object.FindObjectsByType<Chicken>(FindObjectsSortMode.None);
        foreach (var chick in chickens) {
            if (chick != null && chick.isGolden && chick.isHidden) {
                Destroy(chick.gameObject);
            }
        }

        PlayerShooter shooter = Object.FindFirstObjectByType<PlayerShooter>();
        if (shooter != null) {
            shooter.enabled = true; // allow shooting again
        }
    }

    public void ForceCancelChallenge() {
        StopAllCoroutines(); // cancel timers
        GameManager.Instance.typingPanel.SetActive(false); // hide prompt
        challengeActive = false;
        TypingState.IsUserTyping = false;
        currentIndex = 0;

        PlayerShooter shooter = Object.FindFirstObjectByType<PlayerShooter>();
        if (shooter != null) {
            shooter.enabled = true;
        }

        TimeManager.ResumeTime(); // resume gameplay
    }

    public void SetRewardItem(ItemType reward) {
        pendingReward = reward; // set reward before challenge starts
    }
}
