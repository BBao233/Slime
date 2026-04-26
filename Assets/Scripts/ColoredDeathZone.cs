using UnityEngine;

public class ColoredDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Slime")) return;

        ColoredGameManager.Instance.OnColoredSlimeHandled(true);

        Destroy(other.gameObject);
    }
}