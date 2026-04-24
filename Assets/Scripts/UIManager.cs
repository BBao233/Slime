using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("场景设置")]
    public string nextSceneName;   // 下一关
    public string quitSceneName;   // 退出跳转（比如主菜单）

    // ▶ 下一关
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    // ▶ 重开当前关
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ▶ 退出（跳转到指定场景）
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
    // ▶ 直接退出程序（关闭应用）
    public void ExitApplication()
    {
        Time.timeScale = 1f;

        Debug.Log("退出程序");

#if UNITY_EDITOR
    // 在Unity编辑器里用这个停止运行
    UnityEditor.EditorApplication.isPlaying = false;
#else
        // 打包后真正关闭应用
        Application.Quit();
#endif
    }
}