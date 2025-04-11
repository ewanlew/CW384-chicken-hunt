using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            float testMaster = PlayerPrefs.GetFloat("MasterVolume", -1);
            float testSFX = PlayerPrefs.GetFloat("SFXVolume", -1);
            float testMusic = PlayerPrefs.GetFloat("MusicVolume", -1);

            Debug.Log($"[AudioManager] PlayerPrefs Load Test:\nMaster: {testMaster}, SFX: {testSFX}, Music: {testMusic}");
        }
    }


    public void SetVolume(string exposedParam, float value) {
        float clamped = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat(exposedParam, Mathf.Log10(clamped) * 20f);
    }

    public void SaveVolumes() {
        float master, sfx, music;

        audioMixer.GetFloat("MasterVolume", out master);
        audioMixer.GetFloat("SFXVolume", out sfx);
        audioMixer.GetFloat("MusicVolume", out music);

        float linearMaster = Mathf.Pow(10f, master / 20f);
        float linearSFX = Mathf.Pow(10f, sfx / 20f);
        float linearMusic = Mathf.Pow(10f, music / 20f);

        PlayerPrefs.SetFloat("MasterVolume", linearMaster);
        PlayerPrefs.SetFloat("SFXVolume", linearSFX);
        PlayerPrefs.SetFloat("MusicVolume", linearMusic);
        PlayerPrefs.Save();

        Debug.Log($"[AudioManager] Saved Volumes:\nMaster: {linearMaster}, SFX: {linearSFX}, Music: {linearMusic}");
    }


}

