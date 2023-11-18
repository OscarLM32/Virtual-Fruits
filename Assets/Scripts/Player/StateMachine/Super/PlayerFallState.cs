using UnityEngine;

namespace Player.StateMachine
{
    //TODO: refactor class name to "PlayerFallingState"
    public class PlayerFallState : PlayerBaseState, IRootState
    {
        private const float MAX_FALL_VELOCITY = -20;
        private const string FALL_ANIMATION = "PlayerFall";

        private const float _minGlidingActivationTime = 0.15f;
        private float _glidingPressedTime = 0f;

        public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            InitializeSubState();
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
            _glidingPressedTime = 0;
        }

        public override void InitializeSubState()
        {
            if (Context.IsJumpDownPlatformPressed)
            {
                SetSubState(PlayerState.JUMP_DOWN_PLATFORM);
                return;
            }

            if (Context.CurrentMovementInput.x == 0)
            {
                SetSubState(PlayerState.IDLE);
            }
            else
            {
                SetSubState(PlayerState.MOVEMENT);
            }
        }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerHit)
            {
                SwitchState(PlayerState.HIT);
                return;
            }

            if (Context.IsAttackPressed && !Context.RequireNewAttackPress && Context.IsWeaponReady)
            {
                SwitchState(PlayerState.ATTACK);
                return;
            }

            if (Context.IsGrounded && !Context.IsJumpingDownPlatform)
            {
                SwitchState(PlayerState.GROUNDED);
            }
            else if (Context.IsJumpPressed && !Context.RequireNewJumpPress && !(Context.Jumped && Context.DoubleJumped))
            {
                SwitchState(PlayerState.JUMPING);
            }
            else if (!Context.Dashed && Context.IsDashPressed && !Context.RequireNewDashPress)
            {
                SwitchState(PlayerState.DASHING);
            }
            else if (_glidingPressedTime >= _minGlidingActivationTime)
            {
                SwitchState(PlayerState.GLIDING);
            }
            else if (Context.IsGrapplingWall)
            {
                SwitchState(PlayerState.GRAPPLING_WALL);
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
                _glidingPressedTime += Time.deltaTime;
            }
        }

        void CheckMaxFallVelocity()
        {
            Vector2 velocity = Context.Rb2D.velocity;
            Context.Rb2D.velocity = new Vector2(velocity.x, Mathf.Max(velocity.y, MAX_FALL_VELOCITY));
        }
    }
}