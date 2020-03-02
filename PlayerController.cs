using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isUnderImpulse = false;
    
    [Header("Basic Settings")]
    public float speed;
    public float acceleration;
    public float deaccelerationMultiplier;
    public float jumpForce;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;
    [HideInInspector] public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;
    
    [Header("Wall Jump Settings")]
    public bool useWallJump;
    [HideInInspector] public bool isAgainstLeftWall;
    [HideInInspector] public bool isAgainstRightWall;
    [HideInInspector] public bool isWallSliding;
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    public float wallSlideMaxSpeed;
    public float wallJumpKnockBack;
    
    [Header("Better Jump Settings")]
    public bool useBetterJump;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
    }


    void FixedUpdate()
    {
        if ((isAgainstLeftWall || isAgainstRightWall) && rb.velocity.y < 0)
        {
            isWallSliding = true;
            if(rb.velocity.y < -wallSlideMaxSpeed)
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideMaxSpeed);
        }else { isWallSliding = false; }
        isAgainstLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, whatIsWall);
        isAgainstRightWall = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, whatIsWall);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        
        moveInput = Input.GetAxisRaw("Horizontal");
        // if(!isUnderImpulse)
        //     rb.velocity = new Vector2(moveInput * speed * Time.fixedDeltaTime, rb.velocity.y);
        if(rb.velocity.x < moveInput * speed && moveInput > 0) { rb.velocity += new Vector2(acceleration, 0); }
        else if(rb.velocity.x > moveInput * speed && moveInput < 0) { rb.velocity -= new Vector2(acceleration, 0); }
        else if(rb.velocity.x != 0 && moveInput == 0) { rb.velocity = new Vector2(rb.velocity.x * deaccelerationMultiplier, rb.velocity.y); }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        } else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void Update()
    {
        if (isGrounded || (isAgainstLeftWall || isAgainstRightWall && useWallJump))
        {
            extraJumps = extraJumpsValue;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if ((isAgainstLeftWall || isAgainstRightWall) && useWallJump)
            {
                if(isAgainstLeftWall) { rb.AddForce(new Vector2(wallJumpKnockBack * rb.mass, 0), ForceMode2D.Impulse); }
                else if(isAgainstRightWall) { rb.AddForce(new Vector2(-wallJumpKnockBack * rb.mass, 0), ForceMode2D.Impulse); }
                if(Input.GetAxisRaw("Horizontal") != 0) { rb.velocity = new Vector2(rb.velocity.x, jumpForce); }
            }
            else if (extraJumps > 0 || isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                if(extraJumps > 0)
                    extraJumps--;
            }
        }
        
        // Better Jump:
        if (useBetterJump)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        scaler = leftWallCheck.localPosition;
        scaler.x *= -1;
        leftWallCheck.localPosition = scaler;
        scaler = rightWallCheck.localPosition;
        scaler.x *= -1;
        rightWallCheck.localPosition = scaler;
    }
}
