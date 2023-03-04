using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        //This is not the propper solution, but i don't know anymore
        Context.Rb2D.velocity = new Vector2(0, Context.Rb2D.velocity.y);
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //TODO: implement some dust particles when the character begins moving and grounded
    }

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if (Context.IsJumpDownPlatformPressed)
        {
            SwitchState(Factory.JumpDownPlatform());
            return;
        }
        if (Context.CurrentMovementInput.x != 0)
        {
            SwitchState(Factory.Movement());
        }
    }
}
