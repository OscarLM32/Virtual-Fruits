using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class PlayerDashingState : PlayerBaseState, IRootState
{
    private enum Sounds
    {
        Dash
    }
    
    private const string DASH_ANIMATION = "PlayerDash";
    private float _timeSpentDashing = 0;
    private Vector2 _lastPosition;

    public PlayerDashingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.debugText.text = "STATE: DASHING";
        _timeSpentDashing = 0;

        InitializeSubState();
        HandleGravity();
        Dash();

        HandleAnimation();
        
        Context.PlayerAudioManager.Play(Sounds.Dash.ToString());
    }

    public override void UpdateState()
    {
        HandleDashTimer();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.LastDashTime = Time.time;
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
        
        if (Context.IsGrapplingWall)
        {
            Context.Dashed = true;
            SwitchState(Factory.GrapplingWall());
            return;
        }
        
        if (!Context.Dashed)
            return;

        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else
        {
            SwitchState(Factory.Falling());
        }
    }

    public void HandleGravity()
    {
        Context.Rb2D.gravityScale = 0; //The player is not affected by gravity while dashing
    }

    public void HandleAnimation()
    {
        Context.PlayerAnimator.Play(DASH_ANIMATION);
    }

    private void Dash()
    {
        //TODO: spawn some particles when the dash begins
        //TODO: make the dash stop when it collides with an object
        Context.Rb2D.velocity = new Vector2(Context.DashSpeed * Context.LastFacingDirection, 0f);
        Context.RequireNewDashPress = true;
    }
    
    private void HandleDashTimer()
    {
        if (_timeSpentDashing > Context.DashTime)
            Context.Dashed = true;
        _timeSpentDashing += Time.deltaTime;
    }
    
}
