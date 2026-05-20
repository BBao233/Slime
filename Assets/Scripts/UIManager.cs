using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("场景设置")]
    public string nextSceneName;   // 下一关
    public string quitSceneName;   // 返回主菜单

    [Header("设置面板")]
    public GameObject settingPanel;

    // 是否暂停中
    private bool isPaused = false;

    void Start()
    {
        // 初始关闭设置面板
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }

        // 确保游戏开始时时间正常
        Time.timeScale = 1f;
    }

    // ====================================
    // 下一关
    // ====================================
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }

    // ====================================
    // 重开当前关
    // ====================================
    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // ====================================
    // 返回主菜单 / 退出场景
    // ====================================
    public void QuitGame()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(quitSceneName))
        {
            SceneManager.LoadScene(quitSceneName);
        }
        else
        {
            Debug.Log("未设置退出场景");
        }
    }

    // ====================================
    // 真正退出程序
    // ====================================
    public void ExitApplication()
    {
        Time.timeScale = 1f;

        Debug.Log("退出程序");

#if UNITY_EDITOR
        // 编辑器停止运行
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 打包后退出
        Application.Quit();
#endif
    }

    // ====================================
    // 设置面板开关
    // ====================================
    public void ToggleSettingPanel()
    {
        if (settingPanel == null)
            return;

        isPaused = !isPaused;

        // 显示/隐藏面板
        settingPanel.SetActive(isPaused);

        // 暂停/恢复时间
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // ====================================
    // 单独打开设置
    // ====================================
    public void OpenSettingPanel()
    {
        if (settingPanel == null)
            return;

        isPaused = true;

        settingPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // ====================================
    // 单独关闭设置
    // ====================================
    public void CloseSettingPanel()
    {
        if (settingPanel == null)
            return;

        isPaused = false;

        settingPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}