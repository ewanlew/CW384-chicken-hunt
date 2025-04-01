using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject chickenPrefab;
    public float spawnInterval = 3f;
    public float scrollRange = 3f;

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
        float screenBottomY = -6f;

        // range of spawn
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float spawnX = Camera.main.transform.position.x + Random.Range(-camHalfWidth - scrollRange, camHalfWidth + scrollRange);
        Vector3 spawnPos = new Vector3(spawnX, -6f, 0f);

        GameObject chicken = Instantiate(chickenPrefab, spawnPos, Quaternion.identity);

        // vector to centre w/ offset
        float toCenter = Camera.main.transform.position.x - spawnX;
        float xVelocity = (toCenter * 0.3f) + Random.Range(-1f, 1f);
        float yVelocity = Random.Range(5f, 8f);

        // init velocity upward -> centre
        chicken.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity, yVelocity);

        // KILL chicken after a lil
        Destroy(chicken, 12f);
    }

}
