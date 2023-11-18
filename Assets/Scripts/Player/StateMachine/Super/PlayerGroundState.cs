using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerGroundState : PlayerBaseState, IRootState
    {
        private static class GroundedAnimations
        {
            public static readonly string IDLE = "PlayerIdle";
            public static readonly string WALK = "PlayerWalk";
            public static readonly string RUN = "PlayerRun";
        }

        private enum Sounds
        {
            Steps,
            Landing
        }

        private const float _stepsCoolDownTime = 0.45f;
        private float _elapsedTime = _stepsCoolDownTime;

        public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            InitializeSubState();

            //Reset all the movement variables
            Context.Jumped = false;
            Context.DoubleJumped = false;
            Context.Dashed = false;
            Context.WallJumped = false;
            Context.WallJumpsCount = 0;

            Context.PlayerAudioManager.Play(Sounds.Landing.ToString());
        }

        public override void UpdateState()
        {
            HandleSteps();
            HandleAnimation();
            CheckSwitchStates();
        }

        public override void ExitState()
        {
        }

        public override void InitializeSubState()
        {
            if (Context.IsJumpingDownPlatform || Context.IsJumpDownPlatformPressed)
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

            if (Context.IsJumpPressed && !Context.RequireNewJumpPress)
            {
                SwitchState(PlayerState.JUMPING);
            }
            else if (!Context.IsGrounded)
            {
                SwitchState(PlayerState.FALLING);
            }
            else if (!Context.Dashed && Context.IsDashPressed && !Context.RequireNewDashPress)
            {
                SwitchState(PlayerState.DASHING);
            }
        }

        public void HandleAnimation()
        {
            if (Context.CurrentMovementInput.x == 0)
                Context.PlayerAnimator.Play(GroundedAnimations.IDLE);
            else if (Context.IsWalking || Context.CurrentMovementInput.x < 0.5 && Context.CurrentMovementInput.x > -0.5)
                Context.PlayerAnimator.Play(GroundedAnimations.WALK);
            else
                Context.PlayerAnimator.Play(GroundedAnimations.RUN);
        }

        public void HandleGravity()
        {
            //Makes no sense to have this at the time being, but may be useful
            //in later versions
        }


        private void HandleSteps()
        {
            if (Context.CurrentMovement == Vector2.zero)
            {
                _elapsedTime = 0;
                return;
            }

            if (_elapsedTime >= _stepsCoolDownTime)
            {
                Context.PlayerAudioManager.Play(Sounds.Steps.ToString());
                _elapsedTime = 0;
            }
            else
                _elapsedTime += Time.deltaTime;
        }
    }
}