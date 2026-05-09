using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    [Header("需要控制的Panel")]
    public GameObject targetPanel;

    // 打开Panel
    public void OpenPanel()
    {
        targetPanel.SetActive(true);
    }

    // 关闭Panel
    public void ClosePanel()
    {
        targetPanel.SetActive(false);
    }
}