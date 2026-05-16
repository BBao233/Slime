using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}