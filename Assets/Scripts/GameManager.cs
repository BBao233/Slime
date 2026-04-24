using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("生命系统")]
    public int maxLife = 3;
    private int currentLife;

    [Header("Slime统计")]
    private int totalSlime = 0;
    private int handledSlime = 0;

    [Header("UI")]
    public GameObject victoryPanel;
    public GameObject gameOverPanel;
    public LifeUI lifeUI;

    private bool isGameEnded = false;

    void Awake()
    {
        Instance = this;

        currentLife = maxLife;
        Time.timeScale = 1f;

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // 初始化生命UI
        if (lifeUI != null)
            lifeUI.UpdateLife(currentLife);
    }

    // 👉 每生成一个Slime调用
    public void RegisterSlime()
    {
        totalSlime++;
        Debug.Log("总Slime: " + totalSlime);
    }

    // 👉 Slime被处理（接住 or 掉落）
    public void OnSlimeHandled(bool isFailed)
    {
        if (isGameEnded) return;

        handledSlime++;
        Debug.Log("处理进度: " + handledSlime + "/" + totalSlime);

        if (isFailed)
        {
            currentLife--;
            Debug.Log("扣血，剩余生命: " + currentLife);

            // 更新生命UI
            if (lifeUI != null)
                lifeUI.UpdateLife(currentLife);

            if (currentLife <= 0)
            {
                GameOver();
                return;
            }
        }

        // 所有Slime处理完 → 胜利
        if (handledSlime >= totalSlime)
        {
            Victory();
        }
    }

    // ================== 状态控制 ==================

    void Victory()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("胜利！");

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void GameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("失败！");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}