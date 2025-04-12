using UnityEngine;

public class FramerateLimiter : MonoBehaviour
{
    private int targetFPS = 60; // current fps cap value
    private bool limitFPS = false; // whether to apply a cap

    void Start()
    {
        // load saved prefs
        limitFPS = PlayerPrefs.GetInt("LimitFPS", 0) == 1;
        targetFPS = PlayerPrefs.GetInt("FPSCap", 60);

        ApplyFramerateSetting(); // apply on start
    }

    public void SetLimitFPS(bool enabled) {
        limitFPS = enabled;
        PlayerPrefs.SetInt("LimitFPS", enabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyFramerateSetting(); // reapply cap
    }

    public void SetFPSCap(int cap){
        targetFPS = cap;
        PlayerPrefs.SetInt("FPSCap", cap);
        PlayerPrefs.Save();

        if (limitFPS) { ApplyFramerateSetting(); } // only apply if active
    }

    private void ApplyFramerateSetting() {
        Application.targetFrameRate = limitFPS ? targetFPS : -1; // -1 = uncapped
        Debug.Log($"[FramerateLimiter] Frame cap {(limitFPS ? "enabled" : "disabled")}, Target = {targetFPS}");
    }
}
