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

        if (spawnTimer >= 5f) // spawn every 5 seconds
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawningBox"))
        {
            // "save" the cat
            GameLogicCode.Instance.CatSaved();
            Destroy(other.gameObject);
        }
    }

}
