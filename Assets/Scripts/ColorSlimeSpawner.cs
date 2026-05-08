using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public Transform spawnPoint;

    [Header("史莱姆图片库")]
    public SlimeSpriteLibrary spriteLibrary;

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
            // 逻辑颜色：匹配判断靠这个
            slime.colorType = color;

            // 视觉动画：根据颜色分配对应帧
            AssignSlimeSprites(slime, color);

            // 出生后播放常态循环动画
            slime.ShowNormal();
        }

        if (ColoredGameManager.Instance != null)
        {
            ColoredGameManager.Instance.RegisterColoredSlime();
        }
    }

    void AssignSlimeSprites(SlimeColor slime, ColorType color)
    {
        if (spriteLibrary == null)
        {
            Debug.LogWarning("ColorSlimeSpawner 没有设置 SlimeSpriteLibrary");
            return;
        }

        // 常态外形随机 A / B
        bool useA = Random.value < 0.5f;

        switch (color)
        {
            case ColorType.Red:
                slime.normalFrames = useA ? spriteLibrary.redNormalFramesA : spriteLibrary.redNormalFramesB;
                slime.jumpFrames = spriteLibrary.redJumpFrames;
                slime.correctFrames = spriteLibrary.redCorrectFrames;
                slime.wrongFrames = spriteLibrary.redWrongFrames;
                break;

            case ColorType.Yellow:
                slime.normalFrames = useA ? spriteLibrary.yellowNormalFramesA : spriteLibrary.yellowNormalFramesB;
                slime.jumpFrames = spriteLibrary.yellowJumpFrames;
                slime.correctFrames = spriteLibrary.yellowCorrectFrames;
                slime.wrongFrames = spriteLibrary.yellowWrongFrames;
                break;

            case ColorType.Blue:
                slime.normalFrames = useA ? spriteLibrary.blueNormalFramesA : spriteLibrary.blueNormalFramesB;
                slime.jumpFrames = spriteLibrary.blueJumpFrames;
                slime.correctFrames = spriteLibrary.blueCorrectFrames;
                slime.wrongFrames = spriteLibrary.blueWrongFrames;
                break;

            case ColorType.Green:
                slime.normalFrames = useA ? spriteLibrary.greenNormalFramesA : spriteLibrary.greenNormalFramesB;
                slime.jumpFrames = spriteLibrary.greenJumpFrames;
                slime.correctFrames = spriteLibrary.greenCorrectFrames;
                slime.wrongFrames = spriteLibrary.greenWrongFrames;
                break;

            case ColorType.Purple:
                slime.normalFrames = useA ? spriteLibrary.purpleNormalFramesA : spriteLibrary.purpleNormalFramesB;
                slime.jumpFrames = spriteLibrary.purpleJumpFrames;
                slime.correctFrames = spriteLibrary.purpleCorrectFrames;
                slime.wrongFrames = spriteLibrary.purpleWrongFrames;
                break;
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