
using DevSystems.StateMachine;

namespace Enemies.Plant
{
    public class PlantIdleState : BaseState<PlantStateMachine>
    {
        public PlantIdleState(PlantStateMachine context) : base(context)
        {
            
        }

        public override void OnEnter()
        {
            //Play idle animation
            //context.animator.Play()
        }

        public override void OnUpdate()
        {
            CheckSwitchState();
        }

        protected override void CheckSwitchState()
        {

        }

        protected override void OnExit()
        {

        }
    }
}