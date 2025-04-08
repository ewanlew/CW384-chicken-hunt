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

    private void Start() {
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

    public void Return() {
        SceneManager.LoadScene(0);
    }
}
