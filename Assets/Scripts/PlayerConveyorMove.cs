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

            // 在传送带上时，玩家可以左右控制加减速
            currentXSpeed += input * accel * Time.fixedDeltaTime;

            // 最小为0，最大为基础速度2倍
            currentXSpeed = Mathf.Clamp(currentXSpeed, 0f, baseSpeed * 2f);

            // 跟随传送带上下移动
            if (currentConveyor != null)
            {
                rb.position = new Vector2(
                    rb.position.x,
                    rb.position.y + currentConveyor.deltaY
                );
            }

            // 检查是否到达传送带右边缘
            CheckEdgeAndJump();
        }
        else
        {

        }

        rb.velocity = new Vector2(currentXSpeed, rb.velocity.y);
    }

    void CheckEdgeAndJump()
    {
        if (currentConveyorCollider == null || playerCollider == null || hasJumped)
            return;

        Bounds beltBounds = currentConveyorCollider.bounds;
        Bounds playerBounds = playerCollider.bounds;

        float playerRightX = playerBounds.max.x;
        float beltRightX = beltBounds.max.x;

        // 史莱姆最右边到达传送带最右边时起跳
        if (playerRightX >= beltRightX)
        {
            hasJumped = true;
            onConveyor = false;
            currentConveyor = null;
            currentConveyorCollider = null;

            // 播放起跳动画：播放一次，然后停在最后一帧
            if (slimeColor != null)
            {
                slimeColor.ShowJump();
            }

	    if (jumpParticlePrefab != null)
	    {
    	    Vector3 spawnPos = transform.position + (Vector3)particleOffset;
    	    GameObject particle = Instantiate(jumpParticlePrefab, spawnPos, Quaternion.identity);
    	    Destroy(particle, 1f);
	    }

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

            // 如果重新碰到传送带，恢复常态循环动画
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