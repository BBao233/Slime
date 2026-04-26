using UnityEngine;

public class PlateDisappear : MonoBehaviour
{
    public float speed = 2f;
    public float topY = 6f;   // 到这个高度就销毁

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 到顶部 → 销毁
        if (transform.position.y >= topY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            Debug.Log("史莱姆被接住");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnSlimeHandled(false);
            }

            Destroy(other.gameObject);
        }
    }
}