using UnityEngine;

public class PlateDisappear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            Debug.Log("史莱姆被接住");

            // ✅ 通知GameManager（关键！）
            GameManager.Instance.OnSlimeHandled(false);

            // ✅ 再销毁
            Destroy(other.gameObject);
        }
    }
}