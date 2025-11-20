using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    [SerializeField] GameObject catPrefab;

    private float spawnTimer = 0f;

    void Start()
    {
        // spawn one cat immediately
        SpawnCat();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 2f) // spawn every 2 seconds
        {
            SpawnCat();
            spawnTimer = 0f;
        }
    }

    public void SpawnCat()
    {
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(-4f, 4f);

        Instantiate(catPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}
