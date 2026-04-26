using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Hurt,
        Death,
        Attack,
        DoubleStrike,
        JumpAttack,
        Shoot,
        JumpShoot,
        SpecialAttack,
        Dash,
        Roll,
        ClimbLadder,
        WallSlide,
        Pull,
        Push,
        LedgeGrabIdle,
        LedgeGrabLand,
        DoubleJump
    }
    
    public PlayerState currentState;
    public AbilityHandler abilityHandler;
    
    private Animator animator;
    
    private Dictionary<PlayerState, string> stateNameDict = new Dictionary<PlayerState, string>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stateNameDict = new Dictionary<PlayerState, string>
        {
            { PlayerState.Idle, "Idle" },
            { PlayerState.Run, "Run" },
            { PlayerState.Jump, "Jump" },
            { PlayerState.Fall, "Fall" },
            { PlayerState.Hurt, "Hurt" },
            { PlayerState.Death, "Death" },
            { PlayerState.Attack, "Attack" },
            { PlayerState.DoubleStrike, "DoubleStrike" },
            { PlayerState.JumpAttack, "JumpAttack" },
            { PlayerState.Shoot, "Shoot" },
            { PlayerState.JumpShoot, "JumpShoot" },
            { PlayerState.SpecialAttack, "SpecialAttack" },
            { PlayerState.Dash, "Dash" },
            { PlayerState.Roll, "Roll" },
            { PlayerState.ClimbLadder, "ClimbLadder" },
            { PlayerState.WallSlide, "WallSlide" },
            { PlayerState.Pull, "Pull" },
            { PlayerState.Push, "Push" },
            { PlayerState.LedgeGrabIdle, "LedgeGrabIdle" },
            { PlayerState.LedgeGrabLand, "LedgeGrabLand" },
            { PlayerState.DoubleJump, "DoubleJump"}
        };
            
        ChangeState(PlayerState.Idle);
    }

    private void ChangeState(PlayerState newState)
    {
        if(currentState == newState) return;
        
        currentState = newState;
        if(stateNameDict.ContainsKey(currentState))
            animator.Play(stateNameDict[currentState]);
    }

    public void IdleState()
    {
        ChangeState(PlayerState.Idle);
    }
    
    public float MovementState(bool isGrounded, float verticalVelocity)
    {
        if(currentState == PlayerState.Death || currentState == PlayerState.Hurt) return 0;
        
        if(!isGrounded) return AirState(verticalVelocity);
        
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            ChangeState(PlayerState.Run);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
        return horizontal;
    }

    private float AirState(float verticalVelocity)
    {
        if(currentState != PlayerState.JumpAttack)
        {
            if (verticalVelocity > 0.1f && currentState != PlayerState.DoubleJump)
            {
                ChangeState(PlayerState.Jump);
            }
            else if (verticalVelocity < -0.1f)
            {
                ChangeState(PlayerState.Fall);
            }
        }
        return Input.GetAxis("Horizontal");
    }
    
    public Vector2 JumpState(float velocityX, float jumpForce)
    {
        ChangeState(PlayerState.Jump);
        return new Vector2(velocityX, jumpForce);
    }

    public Vector2 DoubleJumpState(float velocityX, float jumpForce)
    {
        ChangeState(PlayerState.DoubleJump);
        return new Vector2(velocityX, jumpForce);
    }
    
    public void FallState()
    {
        ChangeState(PlayerState.Fall);
    }

    public bool LandedState()
    {
        if(currentState == PlayerState.Fall) return true;
        return false;
    }

    public void AttackState(bool isGrounded, int airActionsRemaining)
    {
        if (!abilityHandler.CanUseBasicAttack()) return;
    
        if(isGrounded)
        {
            ChangeState(PlayerState.Attack);
            abilityHandler.ExecuteBasicAttack();
        }
        else
        {
            if(airActionsRemaining > 0)
            {
                ChangeState(PlayerState.JumpAttack);
                abilityHandler.ExecuteBasicAttack();
            }
        }
    }
    
    public void SpecialAttackState()
    {
        if (!abilityHandler.CanUseSpecial()) return;

        ChangeState(PlayerState.SpecialAttack);
        abilityHandler.ExecuteSpecialAttack();
    }

    public void EnhanceState()
    {
        if (!abilityHandler.CanUseBuff()) return;
        
        abilityHandler.ExecuteBuffSkill();
    }
    
    public void HurtState()
    {
        ChangeState(PlayerState.Hurt);
    }
    
    public void DeathState()
    {
        ChangeState(PlayerState.Death);
    }
    
    public bool IsInActionState()
    {
        return currentState == PlayerState.Attack ||
               currentState == PlayerState.DoubleStrike ||
               currentState == PlayerState.SpecialAttack ||
               currentState == PlayerState.Roll ||
               currentState == PlayerState.Dash; 
    }
}
