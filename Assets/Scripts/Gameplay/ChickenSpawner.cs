using UnityEngine;
using System.Collections;

public class ChickenSpawner : MonoBehaviour
{
    [Header("Chicken Prefabs")] 
    [SerializeField] private GameObject chickenPrefab; // regular chicken
    [SerializeField] private GameObject goldenChickenPrefab; // golden chicken
    [SerializeField] private GameObject warningIconPrefab; // UI icon shown before spawn

    [Header("Spawn Offsets & Velocity")]
    [SerializeField] private float xSpawnPadding = 1f; // how far off screen spawn can be
    [SerializeField] private float baseXVelocity = 0.8f; // influence to fly to middle
    [SerializeField] private float minXOffset = -1f;
    [SerializeField] private float maxXOffset = 1f;

    [SerializeField] private float minYOffset = 5f;
    [SerializeField] private float maxYOffset = 8f;

    [Header("Chicken Spin")]
    [SerializeField] private float minSpin = -5f;
    [SerializeField] private float maxSpin = 5f;

    [Header("Spawn Timing & Behaviour")] 
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float scrollRange = 3f;
    [SerializeField] private float chickenLifespanSecs = 12f;
    [SerializeField, Range(0f, 1f)] private float goldenChickenChance = 0.05f;

    private float nextSpawnTime;

    void Update() {
        // only spawn when it's time and game is unpaused
        if (Time.unscaledTime >= nextSpawnTime){
            SpawnChicken();
            nextSpawnTime = Time.unscaledTime + spawnInterval;
        }
    }

    void SpawnChicken() {
        StartCoroutine(SpawnChickenWithWarning()); // warning before spawn
    }

    private IEnumerator SpawnChickenWithWarning() {
        if (!PauseState.IsGamePaused && !TypingState.IsUserTyping ){
            // pick a random spawn x offset from current camera
            float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            float spawnX = Camera.main.transform.position.x + Random.Range(
                -camHalfWidth - scrollRange - xSpawnPadding, 
                camHalfWidth + scrollRange + xSpawnPadding);
            Vector3 spawnPos = new Vector3(spawnX, -6f, 0f); // below screen

            // place warning icon slightly above chicken spawn
            Vector3 warningWorldPos = spawnPos + Vector3.up * 1.6f;

            // clamp icon into camera view horizontally
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(warningWorldPos);
            viewportPos.x = Mathf.Clamp01(viewportPos.x);

            Vector3 clampedWorldPos = Camera.main.ViewportToWorldPoint(viewportPos);
            clampedWorldPos.z = 0f;

            GameObject warning = Instantiate(warningIconPrefab, clampedWorldPos, Quaternion.identity);

            yield return new WaitForSecondsRealtime(0.5f); // hold icon briefly

            Destroy(warning); // fade or destroy handled separately

            GameObject prefabToSpawn;

            // if item slots are full, golden is blocked
            if (ItemManager.Instance != null && ItemManager.Instance.IsFull()) {
                prefabToSpawn = chickenPrefab;
            } else {
                bool isGolden = Random.value <= goldenChickenChance;
                prefabToSpawn = isGolden ? goldenChickenPrefab : chickenPrefab;

                if (isGolden) {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenShineClip);
                }
            }

            AudioManager.Instance.PlaySFX(AudioManager.Instance.chickenSpawnClip);

            GameObject chicken = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            // shoot chicken toward centre with variation
            float toCenter = Camera.main.transform.position.x - spawnX;
            float xVelocity = (toCenter * baseXVelocity) + Random.Range(minXOffset, maxXOffset);
            float yVelocity = Random.Range(minYOffset, maxYOffset);

            Rigidbody2D rb = chicken.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);

            float spin = Random.Range(minSpin, maxSpin);
            rb.AddTorque(spin, ForceMode2D.Impulse);

            // auto destroy after lifespan
            StartCoroutine(DestroyChickenAfterTime(chicken, chickenLifespanSecs));
        }
    }

    private IEnumerator DestroyChickenAfterTime(GameObject chicken, float time) {
        float elapsed = 0f;
        while (elapsed < time) {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Destroy(chicken); // clean up
    }
}
