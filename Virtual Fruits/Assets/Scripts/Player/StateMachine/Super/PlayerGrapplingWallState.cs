using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplingWallState : PlayerBaseState, IRootState
{
    private enum Sounds
    {
        GrappleWall
    }
    
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

        Context.Jumped = true;
        Context.Dashed = false;
        Context.WallJumped = false;
        
        HandleAnimation();
        
        Context.PlayerAudioManager.Play(Sounds.GrappleWall.ToString());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.DoubleJumped = false;
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Empty());
    }

    public override void CheckSwitchStates()
    {
        if (Context.PlayerHit)
        {
            SwitchState(Factory.Hit());
            return;
        }
        
        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Context.IsJumpPressed && !Context.RequireNewJumpPress && (Context.WallJumpsCount < Context.MaxWallJumps))
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

    public void HandleAnimation()
    {
        Context.PlayerAnimator.Play(WALL_GRAPPLING_ANIMATION);
    }
}