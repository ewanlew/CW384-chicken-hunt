using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    public AudioClip menuClickClip;

    [Header("Gameplay")]
    public AudioClip chickenShineClip;
    public AudioClip chickenSpawnClip;
    public AudioClip discardItem;
    public AudioClip addHealth;
    public AudioClip increaseLeaderboard;
    public AudioClip loseGame;
    public AudioClip useItem;
    public AudioClip[] chickenHit;
    public AudioClip chickenFall;
    public AudioClip typingChallengeStart;
    public AudioClip typingChallengeFail;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadVolumes();
            PlayMusic(backgroundMusic);
        } else {
            Destroy(gameObject);
        }
    }

    public void SetVolume(string exposedParam, float linearValue) {
        Debug.Log($"[SetVolume DEBUG] Param: {exposedParam}\n" +
          $"Linear Value (Raw): {linearValue}\n" +
          $"Clamped: {Mathf.Clamp01(linearValue)}\n" +
          $"Log10: {Mathf.Log10(Mathf.Clamp(linearValue, 0.0001f, 1f))}\n" +
          $"Converted dB: {Mathf.Clamp(Mathf.Log10(Mathf.Clamp(linearValue, 0.0001f, 1f)) * 20f, -40f, 0f)}\n" +
          $"AudioMixer Group Volume (before): {GetMixerVolume(exposedParam)}\n" +
          $"Is MusicSource playing: {(musicSource != null ? musicSource.isPlaying.ToString() : "N/A")}\n" +
          $"Clip on musicSource: {(musicSource != null ? (musicSource.clip != null ? musicSource.clip.name : "null") : "null")}");

        linearValue = Mathf.Clamp01(linearValue); // clamp to [0, 1]

        // Convert linear [0.0001 → 1] to dB [-80 → 0]
        float dB;
        if (linearValue <= 0.0001f) {
            dB = -80f; // practically mute
        } else {
            dB = Mathf.Log10(linearValue) * 20f;
            dB = Mathf.Clamp(dB, -40f, 0f);
        }

        audioMixer.SetFloat(exposedParam, dB);

        // Save the 0–1 value to prefs
        PlayerPrefs.SetFloat(exposedParam + "_Saved", linearValue);
        PlayerPrefs.Save();

        if (exposedParam == "MusicVolume" && musicSource != null && !musicSource.isPlaying) {
            musicSource.Play();
        }
    }

    private float GetMixerVolume(string param) {
        if (audioMixer.GetFloat(param, out float val)) {
            return val;
        }
        return float.NaN;
    }


    public float GetSavedVolume(string exposedParam, float fallback = 1f) {
        return PlayerPrefs.GetFloat(exposedParam + "_Saved", fallback);
    }

    public void LoadVolumes() {
        SetVolume("MasterVolume", GetSavedVolume("MasterVolume", 1f));
        SetVolume("SFXVolume", GetSavedVolume("SFXVolume", 0.5f));
        SetVolume("MusicVolume", GetSavedVolume("MusicVolume", 0.5f));
    }

    public void PlayMusic(AudioClip clip) {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
