using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateDisappear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("小球碰到踏板，小球消失");

            // 让小球消失
            other.gameObject.SetActive(false);
        }
    }
}
