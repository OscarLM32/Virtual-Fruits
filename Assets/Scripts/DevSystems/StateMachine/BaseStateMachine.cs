using UnityEngine;

namespace DevSystems.StateMachine
{
    public abstract class BaseStateMachine<T> : MonoBehaviour where T : BaseStateMachine<T>
    {
        public BaseState<T> CurrentState { get; set; }

        protected void Update()
        {
            CurrentState.OnUpdate();
        }
    }
}