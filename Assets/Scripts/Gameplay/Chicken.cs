using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour
{
    public float killRadius = 0.5f; // distance needed to count as a hit
    public bool isGolden = false; // if this one's a golden chicken
    public bool isHidden = false; // used to block interaction during challenge
    [SerializeField] private GameObject hitParticlesPrefab; // feather burst fx

    void Update() {
        // if chicken drops below the screen, count as miss
        if (transform.position.y < -6f) {
            GameManager.Instance.Miss(false); // full miss
            AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenFall); // play fall sound
            Destroy(gameObject); // clean up
        }
    }

    public void TryHit(Vector2 clickPosition) { 
        float distance = Vector2.Distance(transform.position, clickPosition); // how close the shot was

        if (distance <= killRadius) {
            GameManager.Instance.AddScore(1); // hit gives 1 point
            StartCoroutine(HitAnimation()); // play shrink + fade
            Screenshake.Instance?.Shake(); // screen bounce

            // play random chicken hit sound
            int audioIndex = Random.Range(0, AudioManager.Instance.chickenHit.Length);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenHit[audioIndex]);

            // spawn hit particles if quality is on
            if (hitParticlesPrefab != null && ParticleManager.Instance.CurrentQuality != ParticleQuality.Off) {
                GameObject fx = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);

                // reduce particle amount if set to less
                if (ParticleManager.Instance.CurrentQuality == ParticleQuality.Less) {
                    var ps = fx.GetComponent<ParticleSystem>();
                    if (ps != null) {
                        var emission = ps.emission;
                        if (emission.burstCount > 0) {
                            ParticleSystem.Burst burst = emission.GetBurst(0);
                            burst.count = 10f; // halved count
                            emission.SetBurst(0, burst);
                        }
                    }
                }
            }

            // if it's a golden one, start typing challenge instead of destroying
            if (isGolden) {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.typingChallengeStart); // sparkle sound
                GameManager.Instance.TriggerTypingChallnge(); // launch the prompt
                GetComponent<SpriteRenderer>().enabled = false; // hide sprite
                GetComponent<Rigidbody2D>().simulated = false; // stop moving
                isHidden = true; // so it won't get cleaned twice
            }
        } else if (distance <= killRadius * 2f) {
            GameManager.Instance.Miss(true); // near miss
            AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenBarelyMissed); // play barely missed sound

        } else {
            GameManager.Instance.Miss(false); // full miss
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

        Destroy(gameObject); // cleanup after animation
    }
}
