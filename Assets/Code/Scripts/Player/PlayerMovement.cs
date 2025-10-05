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
    [SerializeField] private int maxAirction = 2;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    [SerializeField] private int airActionsRemaining;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        rb = GetComponent<Rigidbody2D>();
        airActionsRemaining = maxAirction;
    }

    void Update()
    {
        HandleAttack(Input.GetKeyDown(KeyCode.J));
        
        if(!playerStateMachine.IsInActionState())
        {
            if(Input.GetKeyDown(KeyCode.Space)) Jump();
        
            horizontalInput = playerStateMachine.MovementState(isGrounded, rb.linearVelocity.y);
        }
        else horizontalInput = 0;
        
        CheckIfGrounded();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            Flip();
        }
    }

    private void HandleAttack(bool inputKey)
    {
        if(inputKey)
        {
            playerStateMachine.AttackState(isGrounded, airActionsRemaining);
            if (!isGrounded && airActionsRemaining > 0) airActionsRemaining--;
        }
    }
    
    private void Jump()
    {
        if (airActionsRemaining > 0)
        {
            if (!isGrounded && airActionsRemaining < maxAirction)
            {
                playerStateMachine.DoubleJumpState();
            }
            else if (isGrounded)
            {
                playerStateMachine.JumpState();
            }
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            airActionsRemaining--;
            isGrounded = false;
        }
    }

    private void CheckIfGrounded()
    {
        bool currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = currentlyGrounded;
        if (isGrounded && playerStateMachine.LandedState())
        {
            playerStateMachine.IdleState();
            airActionsRemaining = 2;
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
