using UnityEngine;
public static class TimeManager {
    public static bool IsTimeFrozen => Time.timeScale == 0f;
    public static bool IsSlowTimeActive {get; private set; } = false;

    public static void SetNormalSpeed() {
        Time.timeScale = 1f;
        IsSlowTimeActive = false;
    }

    public static void SetSlowTime(float scale = 0.5f) {
        Time.timeScale = scale;
        IsSlowTimeActive = true;
    }

    public static void FreezeTime() {
        Time.timeScale = 0f;
    }

    public static void ResumeTime() {
        Time.timeScale = IsSlowTimeActive ? 0.5f : 1f;
    }
}
