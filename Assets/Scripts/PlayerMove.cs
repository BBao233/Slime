using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConveyorMove : MonoBehaviour
{
    [Header("基础速度")]
    public float baseSpeed = 3f;

    [Header("加速强度")]
    public float accel = 5f;

    [Header("离开后的减速")]
    public float slowDownRate = 5f;

    [Header("跳跃力度")]
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private float currentXSpeed;
    private bool onConveyor = false;
    private bool hasJumped = false;

    private ConveyorMove currentConveyor;
    private Collider2D currentConveyorCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        currentXSpeed = baseSpeed;
    }

    void FixedUpdate()
    {
        if (onConveyor)
        {
            float input = Input.GetAxis("Horizontal");

            // 在传送带上时，玩家可以左右控制加减速
            currentXSpeed += input * accel * Time.fixedDeltaTime;

            // 限制速度范围：
            // 最小为0，表示最多减速到停下，不会反向往左
            // 最大为基础速度的2倍
            currentXSpeed = Mathf.Clamp(currentXSpeed, 0f, baseSpeed * 2f);

            // 跟随传送带上下移动
            if (currentConveyor != null)
            {
                rb.position = new Vector2(rb.position.x, rb.position.y + currentConveyor.deltaY);
            }

            // 检查是否到达传送带右边缘
            CheckEdgeAndJump();
        }
        else
        {

        }

        // 水平速度始终生效，竖直速度交给重力/跳跃
        rb.velocity = new Vector2(currentXSpeed, rb.velocity.y);
    }

    void CheckEdgeAndJump()
    {
        if (currentConveyorCollider == null || playerCollider == null || hasJumped)
            return;

        Bounds beltBounds = currentConveyorCollider.bounds;
        Bounds playerBounds = playerCollider.bounds;

        // 小球最右边碰到传送带最右边时起跳
        float playerRightX = playerBounds.max.x;
        float beltRightX = beltBounds.max.x;

        if (playerRightX >= beltRightX)
        {
            hasJumped = true;
            onConveyor = false;
            currentConveyor = null;
            currentConveyorCollider = null;

            // 给固定向上的起跳速度
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            onConveyor = true;
            hasJumped = false;
            currentXSpeed = baseSpeed;

            currentConveyor = collision.gameObject.GetComponent<ConveyorMove>();
            currentConveyorCollider = collision.gameObject.GetComponent<Collider2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            // 正常离开传送带时取消跟随
            if (!hasJumped)
            {
                onConveyor = false;
                currentConveyor = null;
                currentConveyorCollider = null;
            }
        }
    }
}