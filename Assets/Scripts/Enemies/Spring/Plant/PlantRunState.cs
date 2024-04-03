
using DevSystems.StateMachine;

namespace Enemies.Plant
{
    public class PlantRunState : BaseState<PlantStateMachine>
    {
        //TODO: implement runAnimation
        private float _speed = 2f;

        public PlantRunState(PlantStateMachine context) : base(context)
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