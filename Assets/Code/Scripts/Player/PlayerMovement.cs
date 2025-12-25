using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private PlayerStateMachine playerStateMachine;
    private float horizontalInput;
    
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int maxAirAction = 2;
    [SerializeField] private int airActionsRemaining;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        rb = GetComponent<Rigidbody2D>();
        airActionsRemaining = maxAirAction;
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
        if (!canMove) horizontalInput = 0;
        
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
            if (!isGrounded && airActionsRemaining < maxAirAction)
            {
                rb.linearVelocity = playerStateMachine.DoubleJumpState(rb.linearVelocity.x, jumpForce);
                airActionsRemaining--;
            }
            else if (isGrounded)
            {
                rb.linearVelocity = playerStateMachine.JumpState(rb.linearVelocity.x, jumpForce);
                airActionsRemaining--;
            }
            
            isGrounded = false;
        }
    }

    protected override void CheckIfGrounded()
    {
        bool currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = currentlyGrounded;
        if (isGrounded && playerStateMachine.LandedState())
        {
            playerStateMachine.IdleState();
            airActionsRemaining = 2;
        }
    }
    
    protected override void Flip()
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
}
