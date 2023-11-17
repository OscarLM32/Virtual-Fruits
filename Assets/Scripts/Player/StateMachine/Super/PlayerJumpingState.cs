using UnityEngine;

public class PlayerJumpingState : PlayerBaseState, IRootState
{
    private enum Sounds
    {
        Jump
    }

    private PlayerStateMachine.WallJumpInformation _currentWallJump;
    private static class JumpingAnimations
    {
        public static readonly string JUMP = "PlayerJump";
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
        if (Context.WallJumpsCount < Context.MaxWallJumps)
        {
            _currentWallJump = Context.WallJumpsData[Context.WallJumpsCount];
        }
        Context.PlayerAudioManager.Play(Sounds.Jump.ToString());
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
        if (Context.IsGrapplingWall)
        {
            SetSubState(Factory.WallJump());
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
        //Whenever the player starts jumping this function comes into play even before the grounded
        //variable switches to false
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

        if (Context.IsGrounded && !Context.IsJumpPressed)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Context.Rb2D.velocity.y < 0f)
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

    private void HandleJump()
    {
        if (Context.IsGrapplingWall && !Context.RequireNewJumpPress)
        {
            HandleWallJump();
        }
        else if (!Context.RequireNewJumpPress)
        {
            HandleNormalJump();
        }
    }

    private void HandleWallJump()
    {
        float xJumpVelocity = _currentWallJump.InitialJumpVelocity * 0.4f;
        float yJumpVelocity = _currentWallJump.InitialJumpVelocity;
        int direction = Context.LastFacingDirection * -1; //The opposite direction

        Context.WallJumped = true;
        Context.IsGrapplingWall = false;
        Context.Rb2D.velocity = new Vector2(xJumpVelocity * direction, yJumpVelocity);
        Context.RequireNewJumpPress = true;
        Context.WallJumpsCount++;
        HandleAnimation();
    }

    private void HandleNormalJump()
    {
        if (!Context.IsJumpPressed || Context.RequireNewJumpPress)
            return;

        Context.WallJumped = false;
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
        if (Context.WallJumped)
        {
            HandleWallJumpGravity();
        }
        else
        {
            HandleNormalJumpGravity();
        }
    }

    private void HandleWallJumpGravity()
    {
        Context.Rb2D.gravityScale = _currentWallJump.GravityFactor;
    }

    private void HandleNormalJumpGravity()
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

    public void HandleAnimation()
    {
        if (Context.DoubleJumped)
        {
            Context.PlayerAnimator.Play(JumpingAnimations.DOUBLE_JUMP);
            return;
        }
        Context.PlayerAnimator.Play(JumpingAnimations.JUMP);
    }
}