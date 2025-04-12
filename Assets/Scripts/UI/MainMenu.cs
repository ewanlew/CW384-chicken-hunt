using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    [SerializeField] private GameObject CreditsUI;
    void Start() {
        if (CreditsUI != null) {
            CreditsUI.SetActive(false);
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions() {
        SceneManager.LoadScene(2);
    }
    
    public void OpenLeaderboards() {
        SceneManager.LoadScene(3);
    }

    public void ShowCredits() {
        if (CreditsUI != null) {
            CreditsUI.SetActive(true);
        }
    }

    public void HideCredits() {
        if (CreditsUI != null) {
            CreditsUI.SetActive(false);
        }
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
