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
        Hit1,
        Hit2,
        Hit3,
        JumpAttack,
        Shoot,
        JumpShoot,
        SpecialAttack,
        Dash,
        Roll,
        ClimbLadder,
        Pull,
        Push,
        DoubleJump
    }
    
    public PlayerState currentState;
    public AbilityHandler abilityHandler;
    
    private Animator animator;

    [Serializable]
    public struct StateAnimationMapping
    {
        public PlayerState state;
        public AnimationClip animation;
    }
    
    public List<StateAnimationMapping> stateAnimations = new List<StateAnimationMapping>();
    
    private Dictionary<PlayerState, AnimationClip> stateNameDict = new Dictionary<PlayerState, AnimationClip>();

    private int comboStep = 0;
    private bool isComboBuffered = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        stateNameDict =  new Dictionary<PlayerState, AnimationClip>();
        foreach (var mapping in stateAnimations)
        {
            if (!stateNameDict.ContainsKey(mapping.state))
            {
                stateNameDict.Add(mapping.state, mapping.animation);
            }
        }
    }

    private void Start()
    { 
        ChangeState(PlayerState.Idle);
    }

    private void ChangeState(PlayerState newState)
    {
        if(currentState == newState) return;
        
        currentState = newState;
        if(stateNameDict.ContainsKey(currentState))
            animator.Play(stateNameDict[currentState].name);
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
        if (currentState == PlayerState.Hit1 || currentState == PlayerState.Hit2)
        {
            isComboBuffered =  true;
            return;
        }

        if (!IsInActionState())
        {
            if (!abilityHandler.CanUseBasicAttack()) return;

            if (abilityHandler.IsBasicAttackRanged)
            {
                if(isGrounded)
                    ChangeState(PlayerState.Shoot);
                else if (airActionsRemaining > 0)
                    ChangeState(PlayerState.JumpShoot);
                
                //abilityHandler.ExecuteBasicAttack();
            }
            else
            {
                if (isGrounded)
                {
                    comboStep = 1;
                    isComboBuffered = false;
                    ChangeState(PlayerState.Hit1);
                }
                else
                {
                    if (airActionsRemaining > 0)
                    {
                        ChangeState(PlayerState.JumpAttack);
                    }
                }
                
                abilityHandler.ExecuteBasicAttack();
            }
        }
    }

    public void CheckCombo()
    {
        if (isComboBuffered)
        {
            isComboBuffered = false;

            if (currentState == PlayerState.Hit1)
            {
                comboStep = 2;
                ChangeState(PlayerState.Hit2);
                abilityHandler.ExecuteBasicAttack();
            }
            else if (currentState == PlayerState.Hit2)
            {
                comboStep = 3;
                ChangeState(PlayerState.Hit3);
                abilityHandler.ExecuteBasicAttack();
            }
        }
        else ResetCombo();
    }

    public void ResetCombo()
    {
        comboStep = 0;
        isComboBuffered = false;
        ChangeState(PlayerState.Idle);
    }
    
    public void ActiveSkillState()
    {
        if (!abilityHandler.CanUseActiveSkill()) return;
        
        if(abilityHandler.currentSkillType == FormSkillType.SpecialAttack)
            ChangeState(PlayerState.SpecialAttack);
        
        abilityHandler.ExecuteActiveSkill();
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
        return currentState == PlayerState.Hit1 ||
               currentState == PlayerState.Hit2 ||
               currentState == PlayerState.Hit3 ||
               currentState == PlayerState.SpecialAttack ||
               currentState == PlayerState.Shoot ||
               currentState == PlayerState.JumpShoot ||
               currentState == PlayerState.Roll ||
               currentState == PlayerState.Dash; 
    }
}
