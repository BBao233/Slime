using UnityEngine;

public class PlateSpawner : MonoBehaviour
{
    public GameObject platePrefab;

    [Header("生成位置")]
    public float spawnY = -6f;
    public float spawnX = -5f;

    [Header("生成间隔")]
    public float spawnInterval = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPlate();
            timer = 0f;
        }
    }

    void SpawnPlate()
    {
        Vector3 pos = new Vector3(spawnX, spawnY, 0f);
        Instantiate(platePrefab, pos, Quaternion.identity);
    }
}