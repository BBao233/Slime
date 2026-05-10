using UnityEngine;
using System.Collections.Generic;

public class ColoredPlateSpawner : MonoBehaviour
{
    [Header("生成预制体")]
    public GameObject platePrefab;

    [Header("生成位置")]
    public float spawnX = -5f;
    public float spawnY = -6f;

    [Header("最小生成间距")]
    public float minSpawnDistance = 4f;

    [Header("本关盒子颜色配置")]
    public ColorType[] plateOrder;

    [Header("是否随机生成顺序")]
    public bool randomizeOrder = false;

    private List<ColorType> finalOrder;

    // 当前生成索引
    private int spawnIndex = 0;

    // 上一个生成的盒子
    private GameObject lastPlate;

    // 上一个盒子的碰撞体
    private Collider2D lastPlateCollider;

    void Start()
    {
        finalOrder = new List<ColorType>(plateOrder);

        // 是否打乱顺序
        if (randomizeOrder)
        {
            Shuffle(finalOrder);
        }

        // 第一个盒子直接生成
        if (finalOrder.Count > 0)
        {
            SpawnPlate(finalOrder[spawnIndex]);
            spawnIndex++;
        }
    }

    void Update()
    {
        // 全部生成完成
        if (finalOrder == null || spawnIndex >= finalOrder.Count)
            return;

        // 没有碰撞体
        if (lastPlateCollider == null)
            return;

        // 获取上一个盒子的顶部Y坐标
        float lastTopY = lastPlateCollider.bounds.max.y;

        // 安全生成距离
        float safeY = spawnY + minSpawnDistance;

        // 当顶部已经离开生成区域足够距离后生成新的
        if (lastTopY >= safeY)
        {
            SpawnPlate(finalOrder[spawnIndex]);
            spawnIndex++;
        }
    }

    void SpawnPlate(ColorType color)
    {
        Vector3 pos = new Vector3(spawnX, spawnY, 0f);

        GameObject obj = Instantiate(platePrefab, pos, Quaternion.identity);

        // 记录最新盒子
        lastPlate = obj;

        // 获取碰撞体
        lastPlateCollider = obj.GetComponent<Collider2D>();

        // 获取颜色脚本
        ColoredPlate plate = obj.GetComponent<ColoredPlate>();

        if (plate == null)
            return;

        // 设置颜色
        plate.plateColor = color;

        // 应用颜色
        plate.ApplyColorExtern();
    }

    // 洗牌算法
    void Shuffle(List<ColorType> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            ColorType temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}