using UnityEngine;

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

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyColor();
    }

[Header("循环位置")]
public float bottomY = -6f;

void Update()
{
    transform.Translate(Vector3.up * speed * Time.deltaTime);

    // 如果没接满，出上屏幕后从下方重新出现
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

        bool isFailed = slime.colorType != plateColor;

        if (ColoredGameManager.Instance != null)
        {
            ColoredGameManager.Instance.OnColoredSlimeHandled(isFailed);
        }

        if (!isFailed)
        {
            currentCount++;

            if (currentCount >= capacity)
            {
                Destroy(gameObject);
            }
        }

        Destroy(other.gameObject);
    }
}