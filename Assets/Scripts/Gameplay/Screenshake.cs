using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour
{
    public static Screenshake Instance; // singleton reference

    private Coroutine shakeRoutine; // running shake coroutine
    private bool shakeEnabled = true; // toggle based on player prefs

    void Awake() {
        Instance = this; // assign instance

        // check saved preference
        int val = PlayerPrefs.GetInt("ScreenShake", 1);
        shakeEnabled = (val == 1); // enable or disable based on saved value
    }

    public void SetEnabled(bool enabled) {
        shakeEnabled = enabled; // externally set whether shaking is allowed
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.1f) {
        if (!shakeEnabled) { return; } // ignore if turned off

        // stop current shake if it's still running
        if (shakeRoutine != null) {
            StopCoroutine(shakeRoutine);
        }

        // start new shake
        shakeRoutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude) {
        float elapsed = 0f;
        Vector3 basePosition = transform.position; // camera’s position before shake

        while (elapsed < duration) {
            // random offset in both x and y
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = basePosition + new Vector3(offsetX, offsetY, 0f); // apply shake

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.position = basePosition; // reset to original position
    }
}
