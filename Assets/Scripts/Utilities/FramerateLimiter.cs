using UnityEngine;

public class FramerateLimiter : MonoBehaviour
{
    private int targetFPS = 60;
    private bool limitFPS = false;

    void Start()
    {
        limitFPS = PlayerPrefs.GetInt("LimitFPS", 0) == 1;
        targetFPS = PlayerPrefs.GetInt("FPSCap", 60);

        ApplyFramerateSetting();
    }

    public void SetLimitFPS(bool enabled) {
        limitFPS = enabled;
        PlayerPrefs.SetInt("LimitFPS", enabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyFramerateSetting();
    }

    public void SetFPSCap(int cap){
        targetFPS = cap;
        PlayerPrefs.SetInt("FPSCap", cap);
        PlayerPrefs.Save();

        if (limitFPS) { ApplyFramerateSetting(); }
    }

    private void ApplyFramerateSetting() {
        Application.targetFrameRate = limitFPS ? targetFPS : -1;
        Debug.Log($"[FramerateLimiter] Frame cap {(limitFPS ? "enabled" : "disabled")}, Target = {targetFPS}");
    }

}
