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

    private SpriteRenderer sr;
    private Collider2D plateCollider;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        plateCollider = GetComponent<Collider2D>();
        ApplyColor();
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

            // 关闭物理模拟，避免继续被重力、碰撞、速度影响
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

        // 6. 如果盒子接满，先关闭得分区，防止继续接别的史莱姆
        bool plateShouldDestroy = !isFailed && currentCount >= capacity;

        if (plateShouldDestroy && plateCollider != null)
        {
            plateCollider.enabled = false;
        }

        // 7. 等动画播完后，再结算和销毁
        StartCoroutine(HandleAfterAnimation(other.gameObject, isFailed, plateShouldDestroy, animTime));
    }

    private IEnumerator HandleAfterAnimation(GameObject slimeObj, bool isFailed, bool plateShouldDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

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
}