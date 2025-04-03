using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour
{
    public float killRadius = 0.5f;

    public void TryHit(Vector2 clickPosition) { 
        float distance = Vector2.Distance(transform.position, clickPosition);

        if (distance <= killRadius) {
            GameManager.Instance.AddScore(1);
            StartCoroutine(HitAnimation());
            Debug.Log("hit!");
        } else {
            GameManager.Instance.Miss();
            Debug.Log("close but missed...");
        }
    }

    IEnumerator HitAnimation() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // stops velocity of chicken
        if (rb) {
            rb.linearVelocity = Vector2.zero; 
        }

        float duration = 0.2f;
        float time = 0f;

        Color startColor = sr.color;
        Vector3 startScale = transform.localScale;

        while (time < duration) {
            float t = time / duration;

            // shrinks chicken
            transform.localScale = Vector3.Lerp(startScale, startScale * 0.2f, t);

            // fade to white-ish
            sr.color = Color.Lerp(startColor, new Color(1f, 1f, 1f, 0.2f), t);

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


}
