using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    [Header("Master Volume")] 
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeLabel;

    [Header("SFX Volume")] 
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeLabel;

    [Header("Music Volume")] 
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeLabel;

    [Header("Video Settings")]
    [SerializeField] private ToggleSwitch fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private ToggleSwitch showFPSToggle;
    [SerializeField] private ToggleSwitch capFPSToggle;
    [SerializeField] private FramerateLimiter framerateLimiter;
    [SerializeField] private Slider fpsCapSlider;
    [SerializeField] private Slider particleSlider;
    [SerializeField] private TextMeshProUGUI particleLabel;
    [SerializeField] private ToggleSwitch screenShakeToggle;

    private void Start() {
        // AUDIO

        // load saved audio levels
        float master = AudioManager.Instance.GetSavedVolume("MasterVolume", 1f);
        float sfx = AudioManager.Instance.GetSavedVolume("SFXVolume", 0.5f);
        float music = AudioManager.Instance.GetSavedVolume("MusicVolume", 0.5f);

        // apply to sliders
        masterVolumeSlider.value = master;
        sfxVolumeSlider.value = sfx;
        musicVolumeSlider.value = music;

        // update display labels
        UpdateVolume(masterVolumeSlider, masterVolumeLabel, master);
        UpdateVolume(sfxVolumeSlider, sfxVolumeLabel, sfx);
        UpdateVolume(musicVolumeSlider, musicVolumeLabel, music);

        // hook up listeners for changes
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // VIDEO

        // fullscreen toggle
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1;
        fullscreenToggle.SetStateAndStartAnimation(fullscreen);
        Screen.fullScreen = fullscreen;

        // resolution dropdown
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);
        resolutionDropdown.value = savedIndex;
        SetResolutionFromDropdown(savedIndex);

        // fps display toggle
        bool showFPS = PlayerPrefs.GetInt("ShowFPS", 0) == 1;
        showFPSToggle.SetStateAndStartAnimation(showFPS);

        // fps cap toggle
        bool capFPS = PlayerPrefs.GetInt("LimitFPS", 0) == 1;
        capFPSToggle.SetStateAndStartAnimation(capFPS);
        fpsCapSlider.interactable = capFPS;

        // fps cap value
        int savedCap = PlayerPrefs.GetInt("FPSCap", 60);
        fpsCapSlider.value = savedCap;
        framerateLimiter.SetFPSCap(savedCap);

        // VFX

        // particle amount slider
        int particleSetting = PlayerPrefs.GetInt("ParticleAmount", (int)ParticleQuality.More);
        particleSlider.value = particleSetting;
        SetParticleAmount(particleSetting);
        particleSlider.onValueChanged.AddListener(OnParticleSliderChanged);

        // screenshake toggle
        bool screenShake = PlayerPrefs.GetInt("ScreenShake", 1) == 1;
        screenShakeToggle.SetStateAndStartAnimation(screenShake);
        Screenshake.Instance.SetEnabled(screenShake);
    }

    public void OnMasterVolumeChanged(float value) {
        AudioManager.Instance.SetVolume("MasterVolume", value); // update master
    }

    public void OnSFXVolumeChanged(float value) {
        AudioManager.Instance.SetVolume("SFXVolume", value); // update sfx
    }

    public void OnMusicVolumeChanged(float value) {
        AudioManager.Instance.SetVolume("MusicVolume", value); // update music
    }

    private void UpdateVolume(Slider slider, TextMeshProUGUI label, float value) {
        slider.value = value;
        int percent = Mathf.RoundToInt(value * 100f);
        label.text = percent + "%"; // display percent
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"[OptionsManager] Fullscreen set to: {isFullscreen}");
    }

    public void SetResolutionFromDropdown(int i) {
        int index = resolutionDropdown.value;

        // switch to resolution
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

    public void OnShowFPSChanged(bool isFPS) {
        PlayerPrefs.SetInt("ShowFPS", isFPS ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log($"[OptionsManager] Display FPS set to: {isFPS}");
    }

    public void OnLimitFPSChanged(bool limit) {
        framerateLimiter.SetLimitFPS(limit); // enable or disable limiter
        fpsCapSlider.interactable = limit; // grey out if off
    }

    public void OnFPSCapChanged(float val) {
        int fps = Mathf.RoundToInt(fpsCapSlider.value);
        framerateLimiter.SetFPSCap(fps); // update value
    }

    public void SetScreenShake(bool enabled) {
        PlayerPrefs.SetInt("ScreenShake", enabled ? 1 : 0);
        PlayerPrefs.Save();
        if (enabled) {
            Screenshake.Instance?.Shake(); // lil test shake
        }
        Debug.Log($"[OptionsManager] Screenshake set to: {enabled}");
    }

    public void SetParticleAmount(int val) {
        ParticleQuality quality = (ParticleQuality) val;

        PlayerPrefs.SetInt("ParticleAmount", val);
        PlayerPrefs.Save();

        ParticleManager.Instance?.ApplyQualitySetting(quality);
        UpdateParticleLabel(val);

        Debug.Log($"[OptionsManager] Particle Amount set to: {quality}");
    }

    public void OnParticleSliderChanged(float value) {
        int clamped = Mathf.RoundToInt(value);
        particleSlider.value = clamped; // snap to step value
        SetParticleAmount(clamped); // apply setting
        UpdateParticleLabel(clamped); // show label
    }

    private void UpdateParticleLabel(int value) {
        // show text based on setting
        switch ((ParticleQuality)value) {
            case ParticleQuality.Off:
                particleLabel.text = "Off";
                break;
            case ParticleQuality.Less:
                particleLabel.text = "Less";
                break;
            case ParticleQuality.More:
                particleLabel.text = "More";
                break;
        }
    }

    public void Return() {
        SceneManager.LoadScene(0); // go back to main menu
    }
}
