using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplingWallState : PlayerBaseState, IRootState
{
    private const string WALL_GRAPPLING_ANIMATION = "PlayerWallGrapple";
    
    public PlayerGrapplingWallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.debugText.text = "STATE: GRAPPLING WALL";
        
        InitializeSubState();
        HandleGravity();
        Context.Rb2D.velocity = new Vector2(0, 0);
        
        Context.Jumped = false;
        Context.DoubleJumped = false;
        Context.Dashed = false;
        

    }

    public override void UpdateState()
    {
        Context.PlayerAnimator.Play(WALL_GRAPPLING_ANIMATION);
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
        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Context.IsJumpPressed && !Context.RequireNewJumpPress)
        {
            SwitchState(Factory.Jumping());
        }
        else if (!Context.IsGrapplingWall) //No need to check if not grounded because I have a sentinel 
        {
            SwitchState(Factory.Falling());
        }
    }

    public void HandleGravity()
    {
        Context.Rb2D.gravityScale = Context.WallGrapplingGravityFactor;
    }
}