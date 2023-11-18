using System;
using System.Collections.Generic;
using UnityEngine;
using static Player.StateMachine.PlayerStateMachine;

namespace Player.StateMachine
{
    public class PlayerJumpingState : PlayerBaseState, IRootState
    {
        private enum Sounds
        {
            Jump
        }

        private static class JumpingAnimations
        {
            public static readonly string JUMP = "PlayerJump";
            public static readonly string DOUBLE_JUMP = "PlayerDoubleJump";
        }

        private float _desiredGravity;

        //JUMP
        private const float _maxJumpHeight = 2.7f;
        private const float _maxJumpTime = 0.7f;
        private float _initialJumpVelocity;

        //DOUBLE JUMP
        private const float _maxDoubleJumpHeight = 1.7f;
        private const float _maxDoubleJumpTime = 0.5f;
        private float _initialDoubleJumpVelocity;
        private float _doubleJumpGravityFactor;

        //WALL JUMP
        public class WallJumpInformation
        {
            public float InitialJumpVelocity;
            public float GravityFactor;
            public float TimeToApex;
        }

        private Dictionary<int, WallJumpInformation> _wallJumpsData = new Dictionary<int, WallJumpInformation>();
        private WallJumpInformation _currentWallJump;

        public PlayerJumpingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
            SetUpJumpAndGravityVariables();
        }

        private void SetUpJumpAndGravityVariables()
        {
            float timeToApex = _maxJumpTime / 2; //The time it takes to reach the highest point
            _desiredGravity = -2 * _maxJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex;
            Context.JumpingGravityFactor = _desiredGravity / Physics.gravity.y;

            //Initialization of double jump variables
            timeToApex = _maxDoubleJumpTime / 2;
            _desiredGravity = -2 * _maxDoubleJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialDoubleJumpVelocity = 2 * _maxDoubleJumpHeight / timeToApex;
            _doubleJumpGravityFactor = _desiredGravity / Physics.gravity.y;

            //Wall grappling jumps
            SetUpWallJumpVariables();
        }

        private void SetUpWallJumpVariables()
        {
            var ratio = 1f / (Context.MaxWallJumps + 1);
            for (int i = 0; i < Context.MaxWallJumps; i++)
            {
                WallJumpInformation info = new WallJumpInformation();
                info.TimeToApex = _maxJumpTime / 2 * (1 - ratio * i);
                var desiredGravity = -2 * _maxJumpHeight * (1 - ratio * i) / (float)Math.Pow(info.TimeToApex, 2);
                info.InitialJumpVelocity = 2 * _maxJumpHeight * (1 - ratio * i) / info.TimeToApex;
                info.GravityFactor = desiredGravity / -9.8f;
                _wallJumpsData.Add(i, info);
            }
        }

        public override void EnterState()
        {
            //TODO: this does not look like good code at all
            if (Context.WallJumpsCount < Context.MaxWallJumps)
            {
                _currentWallJump = _wallJumpsData[Context.WallJumpsCount];
                Context.CurrentWallJumpTimeToApex = _currentWallJump.TimeToApex;
            }

            Context.PlayerAudioManager.Play(Sounds.Jump.ToString());

            InitializeSubState();
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
                SetSubState(PlayerState.WALL_JUMP);
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
            //Whenever the player starts jumping this function comes into play even before the grounded
            //variable switches to false
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

            if (Context.IsGrounded && !Context.IsJumpPressed)
            {
                SwitchState(PlayerState.GROUNDED);
            }
            else if (Context.Rb2D.velocity.y < 0f)
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
                Context.Rb2D.velocity = new Vector2(Context.Rb2D.velocity.x, _initialJumpVelocity);
                Context.RequireNewJumpPress = true;
                Context.Jumped = true;
                HandleAnimation();
                return;
            }

            if (!Context.DoubleJumped)
            {
                Context.Rb2D.velocity = new Vector2(Context.Rb2D.velocity.x, _initialDoubleJumpVelocity);
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
                Context.Rb2D.gravityScale = _doubleJumpGravityFactor;
                return;
            }
            if (Context.Jumped && Context.IsJumpPressed)
            {
                Context.Rb2D.gravityScale = Context.JumpingGravityFactor;
                return;
            }
            
            //We wan to set the gravityscale to falling gravity factor so if the player releases the jumping key
            //the greater falling gravity affects the jump and the players gets better control on the jumping
            //while still inside the jumping state since tge character has positive y speed
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
}