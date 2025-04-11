using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start() {
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

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
