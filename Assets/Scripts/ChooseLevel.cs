using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectPanel : MonoBehaviour
{
    [Header("关卡选择Panel")]
    public GameObject levelPanel;

    // 打开Panel
    public void OpenPanel()
    {
        levelPanel.SetActive(true);
    }

    // 关闭Panel
    public void ClosePanel()
    {
        levelPanel.SetActive(false);
    }

    // 跳转关卡
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}