using UnityEngine;
using System.Collections;

public class SlimeColor : MonoBehaviour
{
    [Header("逻辑颜色")]
    public ColorType colorType;

    [Header("常态动画帧")]
    public Sprite[] normalFrames;

    [Header("起跳动画帧")]
    public Sprite[] jumpFrames;

    [Header("正确动画帧")]
    public Sprite[] correctFrames;

    [Header("错误动画帧")]
    public Sprite[] wrongFrames;

    [Header("常态动画速度")]
    public float normalFrameInterval = 0.15f;

    [Header("动作动画速度")]
    public float actionFrameInterval = 0.1f;

    private SpriteRenderer sr;
    private Coroutine animationCoroutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // 在传送带上：循环播放
    public void ShowNormal()
    {
        StopCurrentAnimation();

        if (normalFrames != null && normalFrames.Length > 0)
        {
            animationCoroutine = StartCoroutine(PlayLoopAnimation(normalFrames, normalFrameInterval));
        }
    }

    // 离开传送带起跳：播放一次，然后停在最后一帧
    public void ShowJump()
    {
        StopCurrentAnimation();

        if (jumpFrames != null && jumpFrames.Length > 0)
        {
            animationCoroutine = StartCoroutine(PlayOnceAndHold(jumpFrames, actionFrameInterval));
        }
    }

    // 正确进入盒子：播放一次，返回动画时长
    public float ShowCorrect()
    {
        StopCurrentAnimation();

        if (correctFrames != null && correctFrames.Length > 0)
        {
            animationCoroutine = StartCoroutine(PlayOnceAndHold(correctFrames, actionFrameInterval));
            return correctFrames.Length * actionFrameInterval;
        }

        return 0.2f;
    }

    // 错误反馈：播放一次，返回动画时长
    public float ShowWrong()
    {
        StopCurrentAnimation();

        if (wrongFrames != null && wrongFrames.Length > 0)
        {
            animationCoroutine = StartCoroutine(PlayOnceAndHold(wrongFrames, actionFrameInterval));
            return wrongFrames.Length * actionFrameInterval;
        }

        return 0.2f;
    }

    private IEnumerator PlayLoopAnimation(Sprite[] frames, float interval)
    {
        int index = 0;

        while (true)
        {
            if (sr != null && frames[index] != null)
            {
                sr.sprite = frames[index];
            }

            index++;

            if (index >= frames.Length)
            {
                index = 0;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator PlayOnceAndHold(Sprite[] frames, float interval)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            if (sr != null && frames[i] != null)
            {
                sr.sprite = frames[i];
            }

            yield return new WaitForSeconds(interval);
        }

        // 播完之后停在最后一帧，直到碰到盒子
        if (sr != null && frames.Length > 0 && frames[frames.Length - 1] != null)
        {
            sr.sprite = frames[frames.Length - 1];
        }
    }

    private void StopCurrentAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }
}