using UnityEngine;
using System.Collections;

public class WarningIcon : MonoBehaviour
{

    [SerializeField] private float fadeInDuration = 0.2f;

    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color c = spriteRenderer.color;
        c.a = 0f;
        spriteRenderer.color = c;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        float timer = 0f;
        while (timer < fadeInDuration) {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeInDuration);
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
            yield return null;
        }
    }
}
