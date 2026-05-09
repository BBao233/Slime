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
        transform.Translate(Vector3.up * speed * Time.deltaTime);

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

        // 1. 关闭史莱姆碰撞，防止重复触发
        Collider2D slimeCollider = other.GetComponent<Collider2D>();
        if (slimeCollider != null)
        {
            slimeCollider.enabled = false;
        }

        // 2. 禁用史莱姆移动脚本，防止下一帧继续写速度
        PlayerConveyorMove moveScript = other.GetComponent<PlayerConveyorMove>();
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }

        // 3. 停止史莱姆自己的物理运动
        Rigidbody2D slimeRb = other.GetComponent<Rigidbody2D>();
        if (slimeRb != null)
        {
            slimeRb.velocity = Vector2.zero;
            slimeRb.angularVelocity = 0f;
            slimeRb.gravityScale = 0f;
            slimeRb.simulated = false;
        }

        // 4. 让史莱姆跟随盒子一起向上移动
        other.transform.SetParent(transform, true);

        // 5. 判断颜色是否匹配
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

        // 6. 判断盒子是否满容量
        bool plateShouldDestroy = !isFailed && currentCount >= capacity;

        if (plateShouldDestroy && plateCollider != null)
        {
            plateCollider.enabled = false;
        }

        // 7. 挤压音效 → 正确/错误音效 → 动画结束后结算和销毁
        StartCoroutine(HandleAfterAnimationAndSound(
            other.gameObject,
            isFailed,
            plateShouldDestroy,
            animTime
        ));
    }

    private IEnumerator HandleAfterAnimationAndSound(
        GameObject slimeObj,
        bool isFailed,
        bool plateShouldDestroy,
        float animTime
    )
    {
        AudioClip squeezeClip = GetRandomSqueezeSound();

        if (audioSource != null && squeezeClip != null)
        {
            audioSource.PlayOneShot(squeezeClip);
            yield return new WaitForSeconds(squeezeClip.length);
        }

        AudioClip resultClip = null;

        if (isFailed)
        {
            resultClip = wrongSound;
        }
        else
        {
            resultClip = plateShouldDestroy ? fullCorrectSound : correctSound;
        }

        if (audioSource != null && resultClip != null)
        {
            audioSource.PlayOneShot(resultClip);
        }

        float waitTime = animTime;

        if (resultClip != null)
        {
            waitTime = Mathf.Max(animTime, resultClip.length);
        }

        yield return new WaitForSeconds(waitTime);

        if (ColoredGameManager.Instance != null)
        {
            ColoredGameManager.Instance.OnColoredSlimeHandled(isFailed);
        }

        Destroy(slimeObj);

        if (plateShouldDestroy)
        {
            Destroy(gameObject);
        }
    }

    private AudioClip GetRandomSqueezeSound()
    {
        if (squeezeSounds == null || squeezeSounds.Length == 0)
        {
            return null;
        }

        int index = Random.Range(0, squeezeSounds.Length);
        return squeezeSounds[index];
    }
}