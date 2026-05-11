using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float minY = -3f;
    public float maxY = 3f;

    [HideInInspector]
    public float deltaY;

    private float lastY;
    private float inputY;

    void Start()
    {
        lastY = transform.position.y;
    }

    void Update()
    {
        // 使用 Raw 输入，取消加减速感
        inputY = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // 直接移动
        pos.y += inputY * moveSpeed * Time.fixedDeltaTime;

        // 限制范围
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

        // 记录Y变化量
        deltaY = transform.position.y - lastY;
        lastY = transform.position.y;
    }
}