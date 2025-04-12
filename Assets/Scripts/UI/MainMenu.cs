using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    [SerializeField] private GameObject CreditsUI; // panel for the credits section

    void Start() {
        if (CreditsUI != null) {
            CreditsUI.SetActive(false); // make sure it's hidden at start
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(1); // load game scene
    }

    public void OpenOptions() {
        SceneManager.LoadScene(2); // load options menu
    }

    public void OpenLeaderboards() {
        SceneManager.LoadScene(3); // show leaderboard scene
    }

    public void ShowCredits() {
        if (CreditsUI != null) {
            CreditsUI.SetActive(true); // show the credits panel
        }
    }

    public void HideCredits() {
        if (CreditsUI != null) {
            CreditsUI.SetActive(false); // hide the credits panel
        }
    }

    public void QuitGame() {
        Application.Quit(); // quit application
        Debug.Log("Game is exiting"); // log for editor
    }
}
