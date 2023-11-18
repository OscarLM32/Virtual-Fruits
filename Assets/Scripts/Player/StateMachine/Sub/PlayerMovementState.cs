using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerMovementState : PlayerBaseState
    {
        private const float _runningSpeed = 6f;
        private const float _walkingSpeed = _runningSpeed * 0.7f;

        public PlayerMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
            //This logic could be called with an event since there is no need to check it every frame
            float speed = Context.IsWalking ? _walkingSpeed : _runningSpeed;
            Context.Rb2D.velocity = new Vector2(Context.CurrentMovementInput.x * speed, Context.Rb2D.velocity.y);
            CheckSwitchStates();
        }

        public override void ExitState() { }

        public override void InitializeSubState() { }

        public override void CheckSwitchStates()
        {
            if (Context.IsJumpDownPlatformPressed)
            {
                SwitchState(PlayerState.JUMP_DOWN_PLATFORM);
                return;
            }
            if (Context.CurrentMovementInput.x == 0 || Context.IsGrapplingWall)
            {
                SwitchState(PlayerState.IDLE);
            }
        }
    }
}