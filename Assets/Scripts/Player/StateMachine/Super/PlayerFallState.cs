using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: refactor class name to "PlayerFallingState"
public class PlayerFallState : PlayerBaseState, IRootState
{
    private const float MAX_FALL_VELOCITY = -20;
    private const string FALL_ANIMATION = "PlayerFall";
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        //Context.debugText.text = "STATE: FALLING";
        HandleAnimation();
        HandleGravity();
        Context.Jumped = true;
        Context.WallJumped = false;
    }

    public override void UpdateState()
    {
        HandleGlidingPressTime();
        CheckMaxFallVelocity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.GlidingPressedTime = 0;
    }

    public override void InitializeSubState()
    {
        if (Context.IsJumpDownPlatformPressed)
        {
            SetSubState(Factory.JumpDownPlatform());
            return;
        }
        
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
        
        if (Context.IsGrounded && !Context.IsJumpingDownPlatform)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Context.IsJumpPressed && !Context.RequireNewJumpPress && !(Context.Jumped && Context.DoubleJumped))
        {
            SwitchState(Factory.Jumping());
        }
        else if (!Context.Dashed && Context.IsDashPressed && !Context.RequireNewDashPress)
        {
            SwitchState(Factory.Dashing());
        }
        else if (Context.GlidingPressedTime >= Context.GlidingActivationTime)
        {
            SwitchState(Factory.Gliding());
        }
        else if (Context.IsGrapplingWall)
        {
            SwitchState(Factory.GrapplingWall());
        }
    }

    public void HandleGravity()
    {
        Context.Rb2D.gravityScale = Context.FallingGravityFactor;
    }

    public void HandleAnimation()
    {
        Context.PlayerAnimator.Play(FALL_ANIMATION);
    }

    void HandleGlidingPressTime()
    {
        if (Context.IsJumpPressed)
        {
            Context.GlidingPressedTime += Time.deltaTime;
        }
    }
    
    void CheckMaxFallVelocity()
    {
        Vector2 velocity = Context.Rb2D.velocity;
        Context.Rb2D.velocity = new Vector2(velocity.x,Mathf.Max(velocity.y, MAX_FALL_VELOCITY));
    }
}