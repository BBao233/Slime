using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 👉 只处理史莱姆
        if (!other.CompareTag("Slime")) return;

        Debug.Log("史莱姆进入死亡区域: " + other.name);

        // 👉 防止没有GameManager时报错
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSlimeHandled(true);
        }
        else
        {
            Debug.LogError("GameManager.Instance 为空！");
        }

        // 👉 销毁史莱姆
        Destroy(other.gameObject);
    }
}