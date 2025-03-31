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
        float x = Camera.main.transform.position.x + Random.Range(-scrollRange, scrollRange);
        Vector3 spawnPos = new Vector3(x, -6f, 0f);
        GameObject chicken = Instantiate(chickenPrefab, spawnPos, Quaternion.identity);

        float upwardSpeed = Random.Range(1.5f, 3f);
        chicken.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, upwardSpeed);

        Destroy(chicken, 10f);
    }

}
