using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private Rigidbody2D rb;
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private float horizontalInput;
    private bool isFacingRight = true;
    [SerializeField] private bool isGrounded;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        horizontalInput = playerStateMachine.MovementState(isGrounded, rb.velocity.y);
        
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
        
        CheckIfGrounded();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            Flip();
        }
    }
    
    public void Jump()
    {
        if (!isGrounded) return;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        playerStateMachine.JumpState();
        isGrounded = false;
    }

    private void CheckIfGrounded()
    {
        bool currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = currentlyGrounded;
        if (isGrounded && playerStateMachine.currentState == PlayerStateMachine.PlayerState.Fall)
        {
            playerStateMachine.IdleState();
        }
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 newScale = transform.localScale;
        newScale.x = (isFacingRight ? 1 : -1 ) * Mathf.Abs(transform.localScale.x);
        transform.localScale = newScale;
        
        // if (keyFollower != null)
        // {
        //     keyFollower.horizontalOffset *= -1;
        // }
    }
    
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
