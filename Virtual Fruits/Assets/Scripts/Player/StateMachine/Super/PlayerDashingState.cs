using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : PlayerBaseState, IRootState
{
    
    private const string DASH_ANIMATION = "PlayerDash";
    private float _timeSpentDashing = 0;
    
    public PlayerDashingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.debugText.text = "STATE: DASHING";
        InitializeSubState();
        HandleDash();
        HandleGravity();
        _timeSpentDashing = 0; //I make sure that this variable is reset
        Context.PlayerAnimator.Play(DASH_ANIMATION);
    }

    public override void UpdateState()
    {
        HandleDashTimer();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Empty());
    }

    public override void CheckSwitchStates()
    {
        if (!Context.Dashed)
            return;

        if (Context.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Context.IsGrapplingWall)
        {
            SwitchState(Factory.GrapplingWall());
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

    private void HandleDash()
    {
        //TODO: spawn some particles when the dash begins
        //TODO: make teh dash stop when it collides with an object
        Context.Rb2D.velocity = new Vector2(Context.DashSpeed * Context.LastFacingDirection, 0f);
    }
    
    private void HandleDashTimer()
    {
        if (_timeSpentDashing > Context.DashTime)
            Context.Dashed = true;
        _timeSpentDashing += Time.deltaTime;
    }
}
