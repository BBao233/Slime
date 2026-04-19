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
        // 输入放在 Update 读
        inputY = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        pos.y += inputY * moveSpeed * Time.fixedDeltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

        deltaY = transform.position.y - lastY;
        lastY = transform.position.y;
    }
}