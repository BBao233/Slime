using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConveyorMove : MonoBehaviour
{
    [Header("基础速度")]
    public float baseSpeed = 3f;

    [Header("加速强度")]
    public float accel = 5f;

    [Header("跳跃力度")]
    public float jumpForce = 5f;

    [Header("起跳粒子")]
    public GameObject jumpParticlePrefab;
    public Vector2 particleOffset = new Vector2(-0.2f, -0.1f);

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SlimeColor slimeColor;

    private float currentXSpeed;
    private bool onConveyor = false;
    private bool hasJumped = false;

    private ConveyorMove currentConveyor;
    private Collider2D currentConveyorCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        slimeColor = GetComponent<SlimeColor>();

        currentXSpeed = baseSpeed;
    }

    void FixedUpdate()
    {
        if (onConveyor)
        {
            float input = Input.GetAxis("Horizontal");

            // 左方向控制加速减速
            currentXSpeed -= input * accel * Time.fixedDeltaTime;

            // 限制速度
            currentXSpeed = Mathf.Clamp(currentXSpeed, 0f, baseSpeed * 2f);

            // 跟随传送带上下移动
            if (currentConveyor != null)
            {
                rb.position = new Vector2(
                    rb.position.x,
                    rb.position.y + currentConveyor.deltaY
                );
            }

            // 检查左边缘起跳
            CheckEdgeAndJump();
        }

        // 向左移动（负速度）
        rb.velocity = new Vector2(-currentXSpeed, rb.velocity.y);
    }

    void CheckEdgeAndJump()
    {
        if (currentConveyorCollider == null || playerCollider == null || hasJumped)
            return;

        Bounds beltBounds = currentConveyorCollider.bounds;
        Bounds playerBounds = playerCollider.bounds;

        float playerLeftX = playerBounds.min.x;
        float beltLeftX = beltBounds.min.x;

        // 左边缘到达传送带左边缘时起跳
        if (playerLeftX <= beltLeftX)
        {
            hasJumped = true;
            onConveyor = false;
            currentConveyor = null;
            currentConveyorCollider = null;

            // 起跳动画
            if (slimeColor != null)
            {
                slimeColor.ShowJump();
            }

            // 起跳粒子
            if (jumpParticlePrefab != null)
            {
                Vector3 spawnPos = transform.position + (Vector3)particleOffset;
                GameObject particle = Instantiate(jumpParticlePrefab, spawnPos, Quaternion.identity);
                Destroy(particle, 1f);
            }

            // 保持向左惯性并向上跳
            rb.velocity = new Vector2(-currentXSpeed, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            onConveyor = true;
            hasJumped = false;

            // 重置基础速度
            currentXSpeed = baseSpeed;

            currentConveyor = collision.gameObject.GetComponent<ConveyorMove>();
            currentConveyorCollider = collision.gameObject.GetComponent<Collider2D>();

            // 恢复普通动画
            if (slimeColor != null)
            {
                slimeColor.ShowNormal();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            if (!hasJumped)
            {
                onConveyor = false;
                currentConveyor = null;
                currentConveyorCollider = null;
            }
        }
    }
}