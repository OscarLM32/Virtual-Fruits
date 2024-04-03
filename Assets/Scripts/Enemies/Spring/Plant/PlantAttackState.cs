
using DevSystems.StateMachine;

namespace Enemies.Plant
{

    public class PlantAttackState : BaseState<PlantStateMachine>
    {
        public PlantAttackState(PlantStateMachine context) : base(context)
        {
        }

        public override void OnEnter()
        {

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