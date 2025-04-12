using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // panel for pause menu

    private bool isPaused = false; // tracks if the game is paused

    void Update()
    {
        // toggle pause when escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame(); // unpause if currently paused
            } else {
                PauseGame(); // pause if currently running
            }
        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false); // hide pause menu
        TimeManager.ResumeTime(); // resume time based on previous speed
        isPaused = false;
        PauseState.IsGamePaused = false; // update global pause flag
    }

    public void PauseGame() {
        pauseMenuUI.SetActive(true); // show pause menu
        TimeManager.FreezeTime(); // freeze time
        isPaused = true;
        PauseState.IsGamePaused = true; // update global pause flag
    }

    public void LoadMainMenu() {
        pauseMenuUI.SetActive(false); // hide menu just in case
        TimeManager.SetNormalSpeed(); // reset time before switching scene
        SceneManager.LoadScene(0); // go back to main menu
    }

    public void QuitGame(){
        Application.Quit(); // exit the game (does nothing in editor)
    }
}
