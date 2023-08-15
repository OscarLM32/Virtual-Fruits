
public abstract class PlayerBaseState
{
    protected bool IsRootState = false;
    protected PlayerStateMachine Context;
    protected PlayerStateFactory Factory;
    protected PlayerBaseState CurrentSubState;
    protected PlayerBaseState CurrentSuperState;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        Context = currentContext;
        Factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void InitializeSubState();

    public abstract void CheckSwitchStates();
    
    
    public void UpdateStates()
    { 
        //TODO: try refs
        if (CurrentSubState != null)
        {
            CurrentSubState.UpdateStates();
        }
        UpdateState();
    }
    
    protected void SwitchState(PlayerBaseState newState)
    {
        //Current state exits state
        ExitState();
        
        //New state enters state
        newState.EnterState();

        //Switch current state of context
        if (IsRootState)
        {
            Context.CurrentState = newState;   
        }else if (CurrentSuperState != null)
        {
            //set the superstate's substate to be the new one.
            CurrentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        CurrentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        CurrentSubState?.ExitState();
        CurrentSubState = newSubState;
        newSubState.SetSuperState(this);
        CurrentSubState.EnterState();
    }
    
    
}
