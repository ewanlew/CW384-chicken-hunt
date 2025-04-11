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


    private void Start() {
        // AUDIO
        float master = Mathf.Clamp(PlayerPrefs.GetFloat("MasterVolume", 1f), 0f, 1f);
        float sfx = Mathf.Clamp(PlayerPrefs.GetFloat("SFXVolume", 0.5f), 0f, 1f);
        float music = Mathf.Clamp(PlayerPrefs.GetFloat("MusicVolume", 0.5f), 0f, 1f);


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

        bool showFPS = PlayerPrefs.GetInt("ShowFPS", 0) == 1;
        showFPSToggle.SetStateAndStartAnimation(showFPS);

        bool capFPS = PlayerPrefs.GetInt("LimitFPS", 0) == 1;
        capFPSToggle.SetStateAndStartAnimation(capFPS);
        fpsCapSlider.interactable = capFPS;

        int savedCap = PlayerPrefs.GetInt("FPSCap", 60);
        fpsCapSlider.value = savedCap;
        framerateLimiter.SetFPSCap(savedCap);

        // VFX

        int particleSetting = PlayerPrefs.GetInt("ParticleAmount", (int)ParticleQuality.More);
        particleSlider.value = particleSetting;
        SetParticleAmount(particleSetting);
        particleSlider.onValueChanged.AddListener(OnParticleSliderChanged);
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

    public void OnShowFPSChanged(bool isFPS) {
        PlayerPrefs.SetInt("ShowFPS", isFPS ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log($"[OptionsManager] Display FPS set to: {isFPS}");
    }

    public void OnLimitFPSChanged(bool limit) {
        framerateLimiter.SetLimitFPS(limit);
        fpsCapSlider.interactable = limit;
    }

    public void OnFPSCapChanged(float val) {
        int fps = Mathf.RoundToInt(fpsCapSlider.value);
        framerateLimiter.SetFPSCap(fps);
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
        particleSlider.value = clamped; // snap to step
        SetParticleAmount(clamped);
        UpdateParticleLabel(clamped);
    }

    private void UpdateParticleLabel(int value) {
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
        SceneManager.LoadScene(0);
    }
}
