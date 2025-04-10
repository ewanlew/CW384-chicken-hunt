using UnityEngine;
using System.Collections;

public class ChickenSpawner : MonoBehaviour
{
    [Header("Prefabs")] 
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject goldenChickenPrefab;
    [SerializeField] private GameObject warningIconPrefab;

    [Header("Spawning Position")] 
    [SerializeField] private float xSpawnPadding = 1f;
    [SerializeField] private float baseXVelocity = 0.8f;
    [SerializeField] private float minXOffset = -1f;
    [SerializeField] private float maxXOffset = 1f;

    [SerializeField] private float minYOffset = 5f;
    [SerializeField] private float maxYOffset = 8f;

    [SerializeField] private float minSpin = -5f;
    [SerializeField] private float maxSpin = 5f;

    [Header("Game Properties")] 
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float scrollRange = 3f;
    [SerializeField] private float chickenLifespanSecs = 12f;
    [SerializeField, Range(0f, 1f)] private float goldenChickenChance = 0.05f;


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

        GameObject prefabToSpawn;

        // if the item bar full force normal chicken
        if (ItemManager.Instance != null && ItemManager.Instance.IsFull()) {
            prefabToSpawn = chickenPrefab;
        } else {
            prefabToSpawn = Random.value <= goldenChickenChance ? goldenChickenPrefab : chickenPrefab;
        }

        GameObject chicken = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);


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
