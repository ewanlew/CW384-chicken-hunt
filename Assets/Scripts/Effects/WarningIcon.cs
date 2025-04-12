using UnityEngine;
using System.Collections;

public class WarningIcon : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 0.2f; // how long it takes to fade in

    private SpriteRenderer spriteRenderer; // reference to the sprite component

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>(); // grab renderer
        Color c = spriteRenderer.color;
        c.a = 0f; // start fully transparent
        spriteRenderer.color = c;

        StartCoroutine(FadeIn()); // begin fade on awake
    }

    private IEnumerator FadeIn() {
        float timer = 0f;
        while (timer < fadeInDuration) {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeInDuration); // progress percent
            Color c = spriteRenderer.color;
            c.a = alpha; // apply current alpha
            spriteRenderer.color = c;
            yield return null;
        }
    }
}
