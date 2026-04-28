using UnityEngine;
using System.Collections.Generic;

public class ColoredPlateSpawner : MonoBehaviour
{
    public GameObject platePrefab;

    public float spawnX = -5f;
    public float spawnY = -6f;
    public float spawnInterval = 2f;

    [Header("本关盒子颜色配置")]
    public ColorType[] plateOrder;

    [Header("是否随机生成顺序")]
    public bool randomizeOrder = false;

    private List<ColorType> finalOrder;
    private int spawnIndex = 0;
    private float timer = 0f;

    void Start()
    {
        finalOrder = new List<ColorType>(plateOrder);

        if (randomizeOrder)
        {
            Shuffle(finalOrder);
        }
    }

    void Update()
    {
        if (finalOrder == null || spawnIndex >= finalOrder.Count) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval || spawnIndex == 0)
        {
            SpawnPlate(finalOrder[spawnIndex]);
            spawnIndex++;
            timer = 0f;
        }
    }

    void SpawnPlate(ColorType color)
    {
        Vector3 pos = new Vector3(spawnX, spawnY, 0f);

        GameObject obj = Instantiate(platePrefab, pos, Quaternion.identity);

        ColoredPlate plate = obj.GetComponent<ColoredPlate>();
        if (plate == null) return;

        plate.plateColor = color;
        plate.ApplyColorExtern();
    }

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