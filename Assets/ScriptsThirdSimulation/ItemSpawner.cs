using UnityEngine;


public class ItemSpawner : MonoBehaviour, ISimulatable
{
    public GameObject[] itemPrefabs;
    public float spawnInterval = 5f;

    public Vector2 spawnAreaMin = new Vector2(-8f, -4f);
    public Vector2 spawnAreaMax = new Vector2(8f, 4f);

    private float timer;

    void Start()
    {
        ThirdSimulateManager.Instance.Register(this);
        timer = 0f;
    }

    public void Simulate(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomItem();
            timer = 0f;
        }
    }

    private void SpawnRandomItem()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0) return;

        int index = Random.Range(0, itemPrefabs.Length);
        Vector2 pos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(itemPrefabs[index], pos, Quaternion.identity);
    }
}

