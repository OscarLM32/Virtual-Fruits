using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState, IRootState
{
    private static class JumpingAnimations
    {
        public static readonly string JUMP        = "PlayerJump";
        public static readonly string DOUBLE_JUMP = "PlayerDoubleJump";
    }

    public PlayerJumpingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;

    }

    public override void EnterState()
    {
        InitializeSubState();
        Context.debugText.text = "STATE: JUMPING";
    }

    public override void UpdateState()
    {
        HandleJump();
        HandleGravity();
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
        //Whenever the player starts jumping this function comes into play even before the grounded
        //variable switches to false
        if (Context.IsGrounded && !Context.IsJumpPressed)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Context.Rb2D.velocity.y < 0f)
        {
            SwitchState(Factory.Falling());
        }
        else if (!Context.Dashed && Context.IsDashPressed)
        {
            SwitchState(Factory.Dashing());
        }
        else if (Context.IsGrapplingWall)
        {
            SwitchState(Factory.GrapplingWall());
        }
    }
    
    //TODO: Create a method that handles the wall jumps

    private void HandleJump()
    {
        if (!Context.IsJumpPressed || Context.RequireNewJumpPress)
            return;
        
        if (!Context.Jumped)
        {
            Context.Rb2D.velocity = new Vector2(Context.Rb2D.velocity.x, Context.InitialJumpVelocity);
            Context.RequireNewJumpPress = true;
            Context.Jumped = true;
            HandleAnimation();
            return;
        }
        
        if (!Context.DoubleJumped)
        {
            Context.Rb2D.velocity = new Vector2(Context.Rb2D.velocity.x, Context.InitialDoubleJumpVelocity);
            Context.RequireNewJumpPress = true;
            Context.DoubleJumped = true;
            HandleAnimation();
        }
    }

    public void HandleGravity()
    {
        if (Context.DoubleJumped && Context.IsJumpPressed)
        {
            Context.Rb2D.gravityScale = Context.DoubleJumpingGravityFactor;
            return;
        }
        if (Context.Jumped && Context.IsJumpPressed)
        {
            Context.Rb2D.gravityScale = Context.JumpingGravityFactor;
            return;
        }
        Context.Rb2D.gravityScale = Context.FallingGravityFactor;

    }

    private void HandleAnimation()
    {
        if (Context.DoubleJumped)
        {
            Context.PlayerAnimator.Play(JumpingAnimations.DOUBLE_JUMP);
            return;
        }
        Context.PlayerAnimator.Play(JumpingAnimations.JUMP);
    }
}