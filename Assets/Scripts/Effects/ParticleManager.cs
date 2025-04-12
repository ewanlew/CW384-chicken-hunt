using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance; // global reference
    public ParticleQuality CurrentQuality { get; private set; } = ParticleQuality.More; // current setting
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject); // kill duplicates
            return;
        }

        // load saved setting or default to 'more'
        int saved = PlayerPrefs.GetInt("ParticleAmount", (int)ParticleQuality.More);
        CurrentQuality = (ParticleQuality)Mathf.Clamp(saved, 0, 2); // clamp to valid range

        Debug.Log($"[ParticleManager] Loaded ParticleQuality: {CurrentQuality}");
    }

    public void ApplyQualitySetting(ParticleQuality quality) {
        CurrentQuality = quality; // apply new value
        Debug.Log($"[ParticleManager] Quality set to: {quality}");
    }
}
