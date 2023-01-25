using UnityEditor.Animations;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState, IRootState
{
    private static class GroundedAnimations
    {
        public static readonly string IDLE = "PlayerIdle";
        public static readonly string WALK = "PlayerWalk";
        public static readonly string RUN  = "PlayerRun";
    }
    
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        
    }

    public override void EnterState()
    {
        Context.debugText.text = "STATE: GROUNDED";
        InitializeSubState();
        Context.Jumped = false;
        Context.DoubleJumped = false;
        Context.Dashed = false;
    }

    public override void UpdateState()
    {
        HandleAnimation();
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
        if (Context.IsJumpPressed && !Context.RequireNewJumpPress)
        {
            SwitchState(Factory.Jumping());
        }
        else if (!Context.IsGrounded)
        {
            SwitchState(Factory.Falling());
        }
        else if (!Context.Dashed && Context.IsDashPressed)
        {
            SwitchState(Factory.Dashing());
        }
    }

    private void HandleAnimation()
    {
        if(Context.CurrentMovementInput.x == 0)
            Context.PlayerAnimator.Play(GroundedAnimations.IDLE);
        else if (Context.IsWalking || (Context.CurrentMovementInput.x < 0.5 && Context.CurrentMovementInput.x > -0.5))
            Context.PlayerAnimator.Play(GroundedAnimations.WALK);
        else
            Context.PlayerAnimator.Play(GroundedAnimations.RUN);
    }

    public void HandleGravity()
    {
        //Makes no sense to have this at the time being, but may be useful
        //in later versions
    }


}
