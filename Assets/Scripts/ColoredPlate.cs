using UnityEngine;

public class ColoredPlate : MonoBehaviour
{
    [Header("当前颜色")]
    public ColorType plateColor;

    [Header("移动")]
    public float speed = 2f;
    public float topY = 6f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyColor();
    }

    void Update()
    {
        // 向上移动
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 到顶部销毁
        if (transform.position.y >= topY)
        {
            Destroy(gameObject);
        }
    }

    // 👉 外部调用设置颜色
    public void ApplyColorExtern()
    {
        ApplyColor();
    }

    void ApplyColor()
    {
        if (sr == null) return;

        switch (plateColor)
        {
            case ColorType.Red: sr.color = Color.red; break;
            case ColorType.Green: sr.color = Color.green; break;
            case ColorType.Blue: sr.color = Color.blue; break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Slime")) return;

        SlimeColor slime = other.GetComponent<SlimeColor>();
        if (slime == null) return;

        bool isFailed = slime.colorType != plateColor;

        ColoredGameManager.Instance.OnColoredSlimeHandled(isFailed);

        // 👉 不管对错都销毁
        Destroy(other.gameObject);
    }
}