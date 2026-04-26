using UnityEngine;

public class ColoredPlateSpawner : MonoBehaviour
{
    public GameObject platePrefab;

    public float spawnX = -5f;
    public float spawnY = -6f;

    public float spawnInterval = 2f;

    private ColorType lastColor;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPlate), 0f, spawnInterval);
    }

    void SpawnPlate()
    {
        Vector3 pos = new Vector3(spawnX, spawnY, 0f);

        GameObject obj = Instantiate(platePrefab, pos, Quaternion.identity);

        ColoredPlate plate = obj.GetComponent<ColoredPlate>();
        if (plate == null) return;

        // 👉 随机颜色（避免连续相同）
        ColorType newColor;

        do
        {
            newColor = (ColorType)Random.Range(0, 3);
        }
        while (newColor == lastColor);

        lastColor = newColor;

        plate.plateColor = newColor;
        plate.ApplyColorExtern();
    }
}