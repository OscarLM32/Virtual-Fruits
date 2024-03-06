using DynamicDifficulty;
using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerDashingState : PlayerBaseState, IRootState
    {
        private enum Sounds
        {
            Dash
        }

        private const string DASH_ANIMATION = "PlayerDash";

        private float _dashDuration = 0.25f;
        private float _dashDistance = 3.5f;
        private float _dashSpeed = 0;
        
        private float _timeSpentDashing = 0;

        public PlayerDashingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, Difficulty difficulty)
            : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
            SetUpDifficultyParameters(difficulty);
        }

        private void SetUpDifficultyParameters(Difficulty difficulty)
        {
            if(difficulty < DynamicDifficultyConstants.baseDifficulty)
            {
                _dashDuration += 0.05f;
                _dashDistance += 0.75f;
            }
            _dashSpeed = _dashDistance / _dashDuration;
        }

        public override void EnterState()
        {
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
        }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerHit)
            {
                SwitchState(PlayerState.HIT);
                return;
            }

            if (Context.IsGrapplingWall)
            {
                Context.Dashed = true;
                SwitchState(PlayerState.GRAPPLING_WALL);
                return;
            }

            //If the player has not finished dashing wait for teh dash to finish
            if (!Context.Dashed)
                return;

            if (Context.IsGrounded)
            {
                SwitchState(PlayerState.GROUNDED);
            }
            else
            {
                SwitchState(PlayerState.FALLING);
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

            Context.Rb2D.velocity = new Vector2(_dashSpeed * Context.LastFacingDirection, 0f);
            Context.RequireNewDashPress = true;
        }

        private void HandleDashTimer()
        {
            if (_timeSpentDashing > _dashDuration)
            {
                Context.Dashed = true;
            }

            _timeSpentDashing += Time.deltaTime;
        }
    }
}