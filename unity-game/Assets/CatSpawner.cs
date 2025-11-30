using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    [SerializeField] GameObject catPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float minX = -9f;
    public float maxX = 9f;

    [Header("Falling Settings")]
    public float fallSpeed = 5f;

    private float spawnTimer = 0f;

    void Start()
    {
        SpawnCat();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnCat();
            spawnTimer = 0f;
        }
    }

    public void SpawnCat()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);

        // spawn the catbox
        GameObject box = Instantiate(catPrefab, spawnPos, Quaternion.identity);

        box.AddComponent<FallingMovement>().speed = fallSpeed;
    }
}


public class FallingMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
