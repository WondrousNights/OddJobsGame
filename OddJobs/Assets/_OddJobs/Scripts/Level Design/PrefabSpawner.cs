using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float spawnRadius = 1f;
    [SerializeField] private bool spawn = true;

    private float timer = 0f;

    private void Update()
    {
        if (!spawn) return;

        timer += Time.deltaTime;
        if (timer < 1 / spawnRate) return;

        timer = 0f;
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        Instantiate(prefab, transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
    }
}
