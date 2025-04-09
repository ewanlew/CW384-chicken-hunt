using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float deltaTime;
    private float updateTimer;
    private const float updateInterval = 0.5f;

    void Update() {
        bool shouldShow = PlayerPrefs.GetInt("ShowFPS", 0) == 1;
        fpsText.enabled = shouldShow;

        if (!shouldShow) { return; }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        updateTimer += Time.unscaledDeltaTime;

        if (updateTimer >= updateInterval) {
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString() + " fps";
            updateTimer = 0f;
        }
    }
}
