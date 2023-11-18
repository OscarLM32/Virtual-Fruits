namespace Player.StateMachine
{
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
            if (CurrentSubState != null)
            {
                CurrentSubState.UpdateStates();
            }
            UpdateState();
        }

        //TODO: pass the state enum value instead of the entire class type
        protected void SwitchState(PlayerState state)
        {
            var newState = Factory.GetState(state);

            //Current state exits state
            ExitState();

            //New state enters state
            newState.EnterState();

            //Switch current state of context
            if (IsRootState)
            {
                Context.CurrentState = newState;
            }
            else if (CurrentSuperState != null)
            {
                //set the superstate's substate to be the new one.
                CurrentSuperState.SetSubState(newState);
            }

            Context.OnStateChange?.Invoke(state);
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

        //I'm not sure if this is a good solution. It is the simplest way I can think of calling "SetSubState",
        //which is being called by many states since super states set their substates "manually" and I don't want
        //to call the Factory every 2 lines.
        protected void SetSubState(PlayerState newSubState)
        {
            var newState = Factory.GetState(newSubState);
            SetSubState(newState);
        }


    }
}