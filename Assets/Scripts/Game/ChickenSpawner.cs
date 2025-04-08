using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject chickenPrefab;
    public float spawnInterval = 3f;
    public float scrollRange = 3f;

    public float xSpawnPadding = 3f;
    public float baseXVelocity = 0.8f;
    public float minXOffset = -1f;
    public float maxXOffset = 1f;

    public float minYOffset = 5f;
    public float maxYOffset = 8f;

    public float minSpin = -5f;
    public float maxSpin = 5f;

    public float chickenLifespanSecs = 12f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime){
            SpawnChicken();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnChicken()
    {
        // range of spawn
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float spawnX = Camera.main.transform.position.x + Random.Range(
            -camHalfWidth - scrollRange - xSpawnPadding, 
            camHalfWidth + scrollRange + xSpawnPadding);
        Vector3 spawnPos = new Vector3(spawnX, -6f, 0f);

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
