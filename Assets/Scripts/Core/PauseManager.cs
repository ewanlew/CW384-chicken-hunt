using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        TimeManager.ResumeTime();
        isPaused = false;
        PauseState.IsGamePaused = false;
    }

    public void PauseGame() {
        pauseMenuUI.SetActive(true);
        TimeManager.FreezeTime();
        isPaused = true;
        PauseState.IsGamePaused = true;
    }

    public void ShowSettings() {

    }

    public void ShowTutorial() {

    }

    public void LoadMainMenu() {
        pauseMenuUI.SetActive(false);
        TimeManager.SetNormalSpeed();
        SceneManager.LoadScene(0);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
