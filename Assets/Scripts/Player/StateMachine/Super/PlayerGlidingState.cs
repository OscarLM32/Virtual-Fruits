using UnityEngine;

namespace Player.StateMachine
{
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

            if (Context.IsGrounded)
            {
                SwitchState(PlayerState.GROUNDED);
            }
            else if (!Context.IsJumpPressed)
            {
                SwitchState(PlayerState.FALLING);
            }
            else if (!Context.Dashed && Context.IsDashPressed && !Context.RequireNewDashPress)
            {
                SwitchState(PlayerState.DASHING);
            }
            else if (Context.IsGrapplingWall)
            {
                SwitchState(PlayerState.GRAPPLING_WALL);
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
            Context.Rb2D.velocity = new Vector2(velocity.x, Mathf.Max(velocity.y, MAX_FALL_VELOCITY));
        }
    }
}