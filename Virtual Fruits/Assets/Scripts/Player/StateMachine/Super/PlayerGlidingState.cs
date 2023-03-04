using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerGlidingState : PlayerBaseState, IRootState
{
    private const float MAX_FALL_VELOCITY = -5;
    private const string GLIDING_ANIMATION = "PlayerGlide";
    public PlayerGlidingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.debugText.text = "STATE: GLIDING";
        InitializeSubState();
        HandleGravity();
        HandleAnimation();
    }

    public override void UpdateState()
    {
        CheckMaxFallVelocity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        if (Context.CurrentMovementInput.x == 0)
        {
            SetSubState(Factory.Idle());
        }
        else
        {
            SetSubState(Factory.Movement());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Context.PlayerHit)
        {
            SwitchState(Factory.Hit());
            return;
        }
        
        if (Context.IsAttackPressed && !Context.RequireNewAttackPress && Context.IsWeaponReady)
        {
            SwitchState(Factory.Attack());
            return;
        }
        
        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (!Context.IsJumpPressed)
        {
            SwitchState(Factory.Falling());
        }
        else if (!Context.Dashed && Context.IsDashPressed && !Context.RequireNewDashPress)
        {
            SwitchState(Factory.Dashing());
        }
        else if (Context.IsGrapplingWall)
        {
            SwitchState(Factory.GrapplingWall());
        }
    }

    public void HandleGravity()
    {
        Context.Rb2D.gravityScale = Context.GlidingGravityFactor;
    }

    public void HandleAnimation()
    {
        Context.PlayerAnimator.Play(GLIDING_ANIMATION);
    }
    
    private void CheckMaxFallVelocity()
    {
        Vector2 velocity = Context.Rb2D.velocity;
        Context.Rb2D.velocity = new Vector2(velocity.x,Mathf.Max(velocity.y, MAX_FALL_VELOCITY));
    }
}
