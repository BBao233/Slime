using UnityEngine;
using System.Collections;

public class ColoredPlate : MonoBehaviour
{
    [Header("当前颜色")]
    public ColorType plateColor;

    [Header("容量")]
    public int capacity = 2;
    private int currentCount = 0;

    [Header("移动")]
    public float speed = 2f;
    public float topY = 6f;

    [Header("循环位置")]
    public float bottomY = -6f;

    [Header("音效")]
    public AudioSource audioSource;

    [Header("挤压音效：每次随机播放一个")]
    public AudioClip[] squeezeSounds;

    [Header("普通正确音效")]
    public AudioClip correctSound;

    [Header("满容量正确音效")]
    public AudioClip fullCorrectSound;

    [Header("错误音效")]
    public AudioClip wrongSound;

    [Header("锤子特效")]
    public GameObject hammerEffectPrefab;

    [Header("锤子生成偏移")]
    public Vector3 hammerOffset = new Vector3(0.5f, 1f, 0f);

    private SpriteRenderer sr;
    private Collider2D plateCollider;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        plateCollider = GetComponent<Collider2D>();

        ApplyColor();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // 盒子持续向上移动
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 到顶部后循环回到底部
        if (transform.position.y >= topY)
        {
            transform.position = new Vector3(
                transform.position.x,
                bottomY,
                transform.position.z
            );
        }
    }

    public void ApplyColorExtern()
    {
        ApplyColor();
    }

    void ApplyColor()
    {
        if (sr == null) return;

        switch (plateColor)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Slime")) return;

        SlimeColor slime = other.GetComponent<SlimeColor>();
        if (slime == null) return;

        // =========================
        // 生成锤子动画
        // =========================
        if (hammerEffectPrefab != null)
        {
            Vector3 hammerPos = other.transform.position + hammerOffset;

            Instantiate(
                hammerEffectPrefab,
                hammerPos,
                Quaternion.identity
            );
        }

        // =========================
        // 关闭史莱姆碰撞
        // =========================
        Collider2D slimeCollider = other.GetComponent<Collider2D>();

        if (slimeCollider != null)
        {
            slimeCollider.enabled = false;
        }

        // =========================
        // 禁用移动脚本
        // =========================
        PlayerConveyorMove moveScript =
            other.GetComponent<PlayerConveyorMove>();

        if (moveScript != null)
        {
            moveScript.enabled = false;
        }

        // =========================
        // 停止物理
        // =========================
        Rigidbody2D slimeRb = other.GetComponent<Rigidbody2D>();

        if (slimeRb != null)
        {
            slimeRb.velocity = Vector2.zero;
            slimeRb.angularVelocity = 0f;
            slimeRb.gravityScale = 0f;
            slimeRb.simulated = false;
        }

        // =========================
        // 跟随盒子移动
        // =========================
        other.transform.SetParent(transform, true);

        // =========================
        // 判断颜色
        // =========================
        bool isFailed = slime.colorType != plateColor;

        float animTime;

        if (isFailed)
        {
            animTime = slime.ShowWrong();
        }
        else
        {
            animTime = slime.ShowCorrect();
            currentCount++;
        }

        // =========================
        // 判断盒子是否满
        // =========================
        bool plateShouldDestroy =
            !isFailed && currentCount >= capacity;

        if (plateShouldDestroy && plateCollider != null)
        {
            plateCollider.enabled = false;
        }

        // =========================
        // 开始结算流程
        // =========================
        StartCoroutine(
            HandleAfterAnimationAndSound(
                other.gameObject,
                isFailed,
                plateShouldDestroy,
                animTime
            )
        );
    }

    private IEnumerator HandleAfterAnimationAndSound(
        GameObject slimeObj,
        bool isFailed,
        bool plateShouldDestroy,
        float animTime
    )
    {
        // =========================
        // 挤压音效
        // =========================
        AudioClip squeezeClip = GetRandomSqueezeSound();

        if (audioSource != null && squeezeClip != null)
        {
            audioSource.PlayOneShot(squeezeClip);

            yield return new WaitForSeconds(
                squeezeClip.length
            );
        }

        // =========================
        // 正确/错误音效
        // =========================
        AudioClip resultClip = null;

        if (isFailed)
        {
            resultClip = wrongSound;
        }
        else
        {
            resultClip =
                plateShouldDestroy
                ? fullCorrectSound
                : correctSound;
        }

        if (audioSource != null && resultClip != null)
        {
            audioSource.PlayOneShot(resultClip);
        }

        // =========================
        // 等待动画结束
        // =========================
        float waitTime = animTime;

        if (resultClip != null)
        {
            waitTime = Mathf.Max(
                animTime,
                resultClip.length
            );
        }

        yield return new WaitForSeconds(waitTime);

        // =========================
        // 通知GameManager
        // =========================
        if (ColoredGameManager.Instance != null)
        {
            ColoredGameManager.Instance
                .OnColoredSlimeHandled(isFailed);
        }

        // =========================
        // 销毁史莱姆
        // =========================
        Destroy(slimeObj);

        // =========================
        // 销毁满容量盒子
        // =========================
        if (plateShouldDestroy)
        {
            Destroy(gameObject);
        }
    }

    private AudioClip GetRandomSqueezeSound()
    {
        if (squeezeSounds == null ||
            squeezeSounds.Length == 0)
        {
            return null;
        }

        int index = Random.Range(
            0,
            squeezeSounds.Length
        );

        return squeezeSounds[index];
    }
}