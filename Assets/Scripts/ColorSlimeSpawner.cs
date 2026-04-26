using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public Transform spawnPoint;

    private List<ColorType> spawnList = new List<ColorType>();

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            spawnList.Add(ColorType.Red);
            spawnList.Add(ColorType.Green);
            spawnList.Add(ColorType.Blue);
        }

        Shuffle(spawnList);

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        foreach (var color in spawnList)
        {
            SpawnSlime(color);
            yield return new WaitForSeconds(1f);
        }

        ColoredGameManager.Instance.OnColoredSpawnFinished();
    }

    void SpawnSlime(ColorType color)
    {
        GameObject obj = Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);

        SlimeColor slime = obj.GetComponent<SlimeColor>();
        slime.colorType = color;

        ColoredGameManager.Instance.RegisterColoredSlime();

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            switch (color)
            {
                case ColorType.Red: sr.color = Color.red; break;
                case ColorType.Green: sr.color = Color.green; break;
                case ColorType.Blue: sr.color = Color.blue; break;
            }
        }
    }

    void Shuffle(List<ColorType> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}