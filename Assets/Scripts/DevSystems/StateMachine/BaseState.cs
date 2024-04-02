
namespace DevSystems.StateMachine
{
    public abstract class BaseState<T> where T : BaseStateMachine<T>
    {
        protected T context;

        public BaseState(T context)
        {
            this.context = context;
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        protected abstract void OnExit();


        protected abstract void CheckSwitchState();

        protected void SwitchState(BaseState<T> newState)
        {
            OnExit();
            newState.OnEnter();
            context.CurrentState = newState;
        }
    }
}