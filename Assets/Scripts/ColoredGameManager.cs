using UnityEngine;

public class ColoredGameManager : MonoBehaviour
{
    public static ColoredGameManager Instance;

    [Header("生命")]
    public int maxLife = 3;
    private int currentLife;

    [Header("Slime统计")]
    private int totalSlime = 0;
    private int handledSlime = 0;

    private bool spawnFinished = false;

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

        if (lifeUI != null)
            lifeUI.UpdateLife(currentLife);
    }

    // 👉 注册Slime
    public void RegisterColoredSlime()
    {
        totalSlime++;
    }

    // 👉 生成完成
    public void OnColoredSpawnFinished()
    {
        spawnFinished = true;
    }

    // 👉 Slime处理
    public void OnColoredSlimeHandled(bool isFailed)
    {
        if (isGameEnded) return;

        handledSlime++;

        if (isFailed)
        {
            currentLife--;

            if (lifeUI != null)
                lifeUI.UpdateLife(currentLife);

            if (currentLife <= 0)
            {
                ColoredGameOver();
                return;
            }
        }

        if (spawnFinished && handledSlime >= totalSlime)
        {
            ColoredVictory();
        }
    }

    void ColoredVictory()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("第二关胜利！");

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void ColoredGameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("第二关失败！");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}