using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour
{
    public float killRadius = 0.5f;
    public bool isGolden = false;
    public bool isHidden = false;
    [SerializeField] private GameObject hitParticlesPrefab;

    void Update() {
        if (transform.position.y < -6f) {
            GameManager.Instance.Miss(false);
            Destroy(gameObject);
        }
    }

    public void TryHit(Vector2 clickPosition) { 
        float distance = Vector2.Distance(transform.position, clickPosition);

        if (distance <= killRadius) {
            if (isGolden) {
                GameManager.Instance.TriggerTypingChallnge();
                GameManager.Instance.AddScore(1);
                
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;

                isHidden = true;
                return;
            }
            GameManager.Instance.AddScore(1);
            if (!isGolden && hitParticlesPrefab != null) {
                Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            }
            StartCoroutine(HitAnimation());
            Debug.Log("hit!");
        } else if (distance <= killRadius * 2f) {
            GameManager.Instance.Miss(true);
            Debug.Log("close but missed...");
        } else {
            GameManager.Instance.Miss(false);
            Debug.Log("missed completely...");
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
