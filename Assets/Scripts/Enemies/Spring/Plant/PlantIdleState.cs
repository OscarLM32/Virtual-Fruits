
using DevSystems.StateMachine;

namespace Enemies.Plant
{
    public class PlantIdleState : BaseState<PlantStateMachine>
    {
        private const string _idleAnimation = "PlantIdle";

        public PlantIdleState(PlantStateMachine context) : base(context)
        {
            
        }

        public override void OnEnter()
        {
            context.animator.Play(_idleAnimation);
        }

        public override void OnUpdate()
        {
            CheckSwitchState();
        }

        protected override void CheckSwitchState()
        {
            if (context.isPlayerInSafeZone)
            {
                SwitchState(context.runState);
                return;
            }

            if (context.isPlayerInAttackRange)
            {
                
            }

        }

        protected override void OnExit()
        {

        }
    }
}