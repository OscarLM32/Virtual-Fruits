using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        float speed = Context.IsWalking ? Context.WalkingSpeed : Context.Speed;
        Context.Rb2D.velocity = new Vector2(Context.CurrentMovementInput.x * speed, Context.Rb2D.velocity.y);
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Context.IsJumpDownPlatformPressed)
        {
            SwitchState(Factory.JumpDownPlatform());
            return;
        }
        if (Context.CurrentMovementInput.x == 0 || Context.IsGrapplingWall)
        {
            SwitchState(Factory.Idle());
        }
    }
}
