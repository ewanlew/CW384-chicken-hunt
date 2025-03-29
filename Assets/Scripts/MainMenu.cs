using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsMenu;

    void Start() {
        SettingsMenu.SetActive(false);
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
