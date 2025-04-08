using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    [Header("Master Volume")] 
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeLabel;

    [Header("SFX Volume")] 
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI sfxVolumeLabel;

    [Header("Music Volume")] 
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeLabel;

    [Header("Video Settings")]
    public ToggleSwitch fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private void Start() {
        // AUDIO
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        masterVolumeSlider.value = master;
        sfxVolumeSlider.value = sfx;
        musicVolumeSlider.value = music;

        UpdateVolume(masterVolumeSlider, masterVolumeLabel, master);
        UpdateVolume(sfxVolumeSlider, sfxVolumeLabel, sfx);
        UpdateVolume(musicVolumeSlider, musicVolumeLabel, music);

        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // VIDEO

        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1;
        fullscreenToggle.SetStateAndStartAnimation(fullscreen);
        Screen.fullScreen = fullscreen;

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);
        resolutionDropdown.value = savedIndex;
        SetResolutionFromDropdown(savedIndex);
    }

    public void OnMasterVolumeChanged(float value) {
        AudioManager.Instance.SetVolume("MasterVolume", value);
        AudioManager.Instance.SaveVolumes();
    }

    public void OnSFXVolumeChanged(float value) {
        AudioManager.Instance.SetVolume("SFXVolume", value);
        AudioManager.Instance.SaveVolumes();
    }

    public void OnMusicVolumeChanged(float value) {
        AudioManager.Instance.SetVolume("MusicVolume", value);
        AudioManager.Instance.SaveVolumes();
    }

    private void UpdateVolume(Slider slider, TextMeshProUGUI label, float value) {
        slider.value = value;
        int percent = Mathf.RoundToInt(value * 100f);
        label.text = percent + "%";
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"[OptionsManager] Fullscreen set to: {isFullscreen}");

    }

    public void SetResolutionFromDropdown(int i) {
        int index = resolutionDropdown.value;
        switch (index) {
            case 0:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
        }

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();

        Debug.Log($"[OptionsManager] Resolution set to index {index}");
    }



    public void Return() {
        SceneManager.LoadScene(0);
    }
}
