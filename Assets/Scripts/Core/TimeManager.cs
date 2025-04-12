using UnityEngine;

public static class TimeManager {
    // returns true if timescale is zero
    public static bool IsTimeFrozen => Time.timeScale == 0f;

    // true if slow time is currently active
    public static bool IsSlowTimeActive {get; private set; } = false;

    public static void SetNormalSpeed() {
        Time.timeScale = 1f; // reset timescale
        IsSlowTimeActive = false; // mark slow time off
    }

    public static void SetSlowTime(float scale = 0.5f) {
        Time.timeScale = scale; // set to custom lower value
        IsSlowTimeActive = true; // track that slow time is active
    }

    public static void FreezeTime() {
        Time.timeScale = 0f; // hard stop for menus/challenges
    }

    public static void ResumeTime() {
        // restore to slow or normal speed depending on state
        Time.timeScale = IsSlowTimeActive ? 0.5f : 1f;
    }
}
