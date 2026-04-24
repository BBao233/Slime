using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [Header("预制体")]
    public GameObject slimePrefab;

    [Header("生成参数")]
    public int totalToSpawn = 10;        // 本关总共生成多少个
    public float spawnInterval = 1.5f;   // 生成间隔

    [Header("生成位置偏移")]
    public Vector2 spawnOffset = new Vector2(0, 1.5f);

    private int spawnedCount = 0;
    private float timer = 0f;
    private bool isSpawning = true;

    void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        if (spawnedCount >= totalToSpawn)
        {
            isSpawning = false;
            return;
        }

        Vector3 spawnPos = transform.position + (Vector3)spawnOffset;
        spawnPos.z = 0;

        GameObject slime = Instantiate(slimePrefab, spawnPos, Quaternion.identity);

        spawnedCount++;

        // 👉 注册到GameManager（关键！）
        GameManager.Instance.RegisterSlime();
    }
}