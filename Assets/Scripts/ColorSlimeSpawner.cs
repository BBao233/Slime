using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public Transform spawnPoint;

    [Header("本关史莱姆颜色配置")]
    public ColorType[] slimeOrder;

    [Header("是否随机生成顺序")]
    public bool randomizeOrder = false;

    [Header("生成间隔")]
    public float spawnInterval = 1f;

    void Start()
    {
        List<ColorType> finalOrder = new List<ColorType>(slimeOrder);

        if (randomizeOrder)
        {
            Shuffle(finalOrder);
        }

        StartCoroutine(SpawnRoutine(finalOrder));
    }

    IEnumerator SpawnRoutine(List<ColorType> order)
    {
        foreach (var color in order)
        {
            SpawnSlime(color);
            yield return new WaitForSeconds(spawnInterval);
        }

        if (ColoredGameManager.Instance != null)
        {
            ColoredGameManager.Instance.OnColoredSpawnFinished();
        }
    }

    void SpawnSlime(ColorType color)
    {
        GameObject obj = Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);

        SlimeColor slime = obj.GetComponent<SlimeColor>();
        if (slime != null)
        {
            slime.colorType = color;
        }

        if (ColoredGameManager.Instance != null)
        {
            ColoredGameManager.Instance.RegisterColoredSlime();
        }

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            switch (color)
            {
                case ColorType.Red:
                    sr.color = Color.red;
                    break;
                case ColorType.Yellow:
                    sr.color = Color.yellow;
                    break;
                case ColorType.Blue:
                    sr.color = Color.blue;
                    break;
                case ColorType.Green:
                    sr.color = Color.green;
                    break;
                case ColorType.Purple:
                    sr.color = new Color(0.55f, 0f, 1f);
                    break;
            }
        }
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