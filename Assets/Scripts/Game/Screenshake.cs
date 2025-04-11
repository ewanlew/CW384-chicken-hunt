using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour
{
    public static Screenshake Instance;

    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    void Awake() {
        Instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.1f) {
        if (shakeRoutine != null) {
            StopCoroutine(shakeRoutine);
        }

        shakeRoutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude) {
        float elapsed = 0f;

        while (elapsed < duration) {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
