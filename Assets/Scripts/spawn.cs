using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject slimePrefab;
    public float spawnInterval = 1.5f;

    [Header("生成位置偏移")]
    public Vector2 spawnOffset = new Vector2(0, 1.5f);

    [Header("数量限制")]
    public int maxSlimeCount = 10;

    private int currentCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1f, spawnInterval);
    }

    void Spawn()
    {
        if (currentCount >= maxSlimeCount) return;

        Vector3 spawnPos = transform.position + (Vector3)spawnOffset;

        GameObject slime = Instantiate(slimePrefab, spawnPos, Quaternion.identity);

        currentCount++;

        // 👇 在生成时直接绑定销毁回调
        SlimeAutoDestroy tracker = slime.AddComponent<SlimeAutoDestroy>();
        tracker.Init(this);
    }

    // 被史莱姆调用
    public void OnSlimeDestroyed()
    {
        currentCount--;
    }

    public class SlimeAutoDestroy : MonoBehaviour
    {
        private SlimeSpawner spawner;

        public void Init(SlimeSpawner s)
        {
            spawner = s;
        }

        void OnDestroy()
        {
            if (spawner != null)
            {
                spawner.OnSlimeDestroyed();
            }
        }
    }

}