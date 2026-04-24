using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public Image[] hearts;

    void Start()
    {
        // 👉 自动初始化（防止一开始UI不对）
        UpdateLife(hearts.Length);
    }

    public void UpdateLife(int currentLife)
    {
        Debug.Log("更新生命UI: " + currentLife);

        if (hearts == null || hearts.Length == 0)
        {
            Debug.LogError("hearts数组没有绑定！");
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                Debug.LogError("hearts数组中有空元素！");
                continue;
            }

            // 核心逻辑：显示 / 隐藏
            hearts[i].gameObject.SetActive(i < currentLife);
        }
    }
}