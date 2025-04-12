using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // global access to audio manager
    public AudioMixer audioMixer; // reference to unity mixer asset

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource; // source for music
    [SerializeField] private AudioSource sfxSource; // source for sound effects

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic; // music that plays on startup
    public AudioClip menuClickClip; // used on buttons

    [Header("Gameplay")]
    public AudioClip chickenShineClip; // when golden chicken appears
    public AudioClip chickenSpawnClip; // normal chicken spawns
    public AudioClip discardItem; // item thrown out
    public AudioClip addHealth; // gain heart
    public AudioClip increaseLeaderboard; // move up on scoreboard
    public AudioClip loseGame; // on game over
    public AudioClip useItem; // using any item
    public AudioClip[] chickenHit; // random hit sounds
    public AudioClip chickenFall; // chicken missed and falls
    public AudioClip typingChallengeStart; // challenge begins
    public AudioClip typingChallengeFail; // wrong input in challenge

    /**
    *   keeps one instance across the game, loads previous saved volumes on load
    *   and plays BGM immediately
    */  
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keep alive between scenes

            LoadVolumes(); // apply saved prefs
            PlayMusic(backgroundMusic); // start bgm
        } else {
            Destroy(gameObject); // kill extras
        }
    }

    public void SetVolume(string exposedParam, float linearValue) {
        // debug logging for tracking weird behaviour
        Debug.Log($"[SetVolume DEBUG] Param: {exposedParam}\n" +
          $"Linear Value (Raw): {linearValue}\n" +
          $"Clamped: {Mathf.Clamp01(linearValue)}\n" +
          $"Log10: {Mathf.Log10(Mathf.Clamp(linearValue, 0.0001f, 1f))}\n" +
          $"Converted dB: {Mathf.Clamp(Mathf.Log10(Mathf.Clamp(linearValue, 0.0001f, 1f)) * 20f, -40f, 0f)}\n" +
          $"AudioMixer Group Volume (before): {GetMixerVolume(exposedParam)}\n" +
          $"Is MusicSource playing: {(musicSource != null ? musicSource.isPlaying.ToString() : "N/A")}\n" +
          $"Clip on musicSource: {(musicSource != null ? (musicSource.clip != null ? musicSource.clip.name : "null") : "null")}");

        linearValue = Mathf.Clamp01(linearValue); // safety check

        // convert to decibels
        float dB;
        if (linearValue <= 0.0001f) {
            dB = -80f; // mute
        } else {
            dB = Mathf.Log10(linearValue) * 20f;
            dB = Mathf.Clamp(dB, -40f, 0f); // cap low-end
        }

        audioMixer.SetFloat(exposedParam, dB); // update exposed param

        // save the linear 0–1 value for next session
        PlayerPrefs.SetFloat(exposedParam + "_Saved", linearValue);
        PlayerPrefs.Save();

        // restart music if volume was changed while paused
        if (exposedParam == "MusicVolume" && musicSource != null && !musicSource.isPlaying) {
            musicSource.Play();
        }
    }

    private float GetMixerVolume(string param) {
        if (audioMixer.GetFloat(param, out float val)) {
            return val; // fetch raw dB
        }
        return float.NaN;
    }

    public float GetSavedVolume(string exposedParam, float fallback = 1f) {
        return PlayerPrefs.GetFloat(exposedParam + "_Saved", fallback); // fallback if missing
    }

    public void LoadVolumes() {
        // load previously saved settings
        float master = PlayerPrefs.GetFloat("MasterVolume_Saved", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume_Saved", 0.5f);
        float music = PlayerPrefs.GetFloat("MusicVolume_Saved", 0.5f);

        SetVolume("MasterVolume", master);
        SetVolume("SFXVolume", sfx);
        SetVolume("MusicVolume", music);

        Debug.Log($"[AudioManager] Loaded volumes from PlayerPrefs:\n" +
                $"- Master: {master}\n" +
                $"- SFX: {sfx}\n" +
                $"- Music: {music}");
    }

    public void PlayMusic(AudioClip clip) {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip; // set track
        musicSource.loop = true; // loop it
        musicSource.Play(); // go
    }

    public void PlaySFX(AudioClip clip) {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip); // fire and forget
    }
}
