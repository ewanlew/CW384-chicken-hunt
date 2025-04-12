using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;
    public ParticleQuality CurrentQuality { get; private set; } = ParticleQuality.More;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        int saved = PlayerPrefs.GetInt("ParticleAmount", (int)ParticleQuality.More);
        CurrentQuality = (ParticleQuality)Mathf.Clamp(saved, 0, 2);

        Debug.Log($"[ParticleManager] Loaded ParticleQuality: {CurrentQuality}");
    }

    public void ApplyQualitySetting(ParticleQuality quality) {
        CurrentQuality = quality;
        Debug.Log($"[ParticleManager] Quality set to: {quality}");
    }
}
