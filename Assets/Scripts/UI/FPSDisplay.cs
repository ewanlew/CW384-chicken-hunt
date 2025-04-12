using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText; // label for fps value

    private float deltaTime; // smoothed delta for fps calc
    private float updateTimer; // how long since last update
    private const float updateInterval = 0.5f; // update rate (every half sec)

    void Update() {
        bool shouldShow = PlayerPrefs.GetInt("ShowFPS", 0) == 1; // read setting
        fpsText.enabled = shouldShow; // hide or show text

        if (!shouldShow) { return; } // skip if disabled

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; // smooth out spikes
        updateTimer += Time.unscaledDeltaTime;

        if (updateTimer >= updateInterval) {
            float fps = 1.0f / deltaTime; // calculate framerate
            fpsText.text = Mathf.Ceil(fps).ToString() + " fps"; // update display
            updateTimer = 0f; // reset
        }
    }
}
