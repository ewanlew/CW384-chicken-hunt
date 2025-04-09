using UnityEngine;
using System.Collections;

public class ChickenSpawner : MonoBehaviour
{
    [Header("Prefabs")] 
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject warningIconPrefab;

    [Header("Spawning Position")] 
    public float xSpawnPadding = 1f;
    public float baseXVelocity = 0.8f;
    public float minXOffset = -1f;
    public float maxXOffset = 1f;

    public float minYOffset = 5f;
    public float maxYOffset = 8f;

    public float minSpin = -5f;
    public float maxSpin = 5f;

    [Header("Game Properties")] 
    public float spawnInterval = 3f;
    public float scrollRange = 3f;
    public float chickenLifespanSecs = 12f;

    private float nextSpawnTime;

    void Update() {
        if (Time.time >= nextSpawnTime){
            SpawnChicken();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnChicken() {
        StartCoroutine(SpawnChickenWithWarning());
    }

    private IEnumerator SpawnChickenWithWarning() {
        // range of spawn
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float spawnX = Camera.main.transform.position.x + Random.Range(
            -camHalfWidth - scrollRange - xSpawnPadding, 
            camHalfWidth + scrollRange + xSpawnPadding);
        Vector3 spawnPos = new Vector3(spawnX, -6f, 0f);

        // ideal spawn point
        Vector3 warningWorldPos = spawnPos + Vector3.up * 1.6f;

        // clamp icon w/in cam view
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(warningWorldPos);
        viewportPos.x = Mathf.Clamp01(viewportPos.x); // clamp horizontal screen space (0–1)

        Vector3 clampedWorldPos = Camera.main.ViewportToWorldPoint(viewportPos);
        clampedWorldPos.z = 0f; // ensure it's in view

        // spawn warning
        GameObject warning = Instantiate(warningIconPrefab, clampedWorldPos, Quaternion.identity);

        yield return new WaitForSeconds(0.5f); // warning display time

        Destroy(warning); // fade is handled separately

        GameObject chicken = Instantiate(chickenPrefab, spawnPos, Quaternion.identity);

        // vector to centre w/ offset
        float toCenter = Camera.main.transform.position.x - spawnX;
        float xVelocity = (toCenter * baseXVelocity) + Random.Range(minXOffset, maxXOffset);
        float yVelocity = Random.Range(minYOffset, maxYOffset);

        // applies vector + rotation
        Rigidbody2D rb = chicken.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);

        float spin = Random.Range(minSpin, maxSpin);
        rb.AddTorque(spin, ForceMode2D.Impulse);

        // KILL chicken after a lil
        Destroy(chicken, chickenLifespanSecs);
    }
}
