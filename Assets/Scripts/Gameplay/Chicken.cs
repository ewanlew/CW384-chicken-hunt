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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenFall);
            Destroy(gameObject);
        }
    }

    public void TryHit(Vector2 clickPosition) { 
    float distance = Vector2.Distance(transform.position, clickPosition);

    if (distance <= killRadius) {
        GameManager.Instance.AddScore(1);
        StartCoroutine(HitAnimation());
        Screenshake.Instance?.Shake();

        int audioIndex = Random.Range(0, AudioManager.Instance.chickenHit.Length);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenHit[audioIndex]);

        if (hitParticlesPrefab != null && ParticleManager.Instance.CurrentQuality != ParticleQuality.Off) {
            GameObject fx = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);

            if (ParticleManager.Instance.CurrentQuality == ParticleQuality.Less) {
                var ps = fx.GetComponent<ParticleSystem>();
                if (ps != null) {
                    var emission = ps.emission;
                    if (emission.burstCount > 0) {
                        ParticleSystem.Burst burst = emission.GetBurst(0);
                        burst.count = 10f;
                        emission.SetBurst(0, burst);
                    }
                }
            }
        }
        if (isGolden) {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.typingChallengeStart);
            GameManager.Instance.TriggerTypingChallnge();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            isHidden = true;
        }
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
