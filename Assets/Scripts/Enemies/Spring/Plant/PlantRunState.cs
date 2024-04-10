
using DevSystems.StateMachine;
using UnityEngine;

namespace Enemies.Plant
{
    public class PlantRunState : BaseState<PlantStateMachine>
    {
        //TODO: implement runAnimation
        private float _speed = 2f;

        public PlantRunState(PlantStateMachine context, float speed) : base(context)
        {
            _speed = speed;
        }

        public override void OnEnter()
        {
            HandleVelocity();
        }

        public override void OnUpdate()
        {
            HandleVelocity();
            CheckSwitchState();
        }

        protected override void CheckSwitchState()
        {
            if(context.isPlayerInAttackRange && !context.canRun)
            {
                SwitchState(context.attackState);
                return;
            }

            if (!context.isPlayerInAttackRange)
            {
                SwitchState(context.idleState);
                return;
            }
        }

        protected override void OnExit()
        {
            context.rb.velocity = Vector2.zero;
        }

        private void HandleVelocity()
        {
            var velocity = new Vector2(_speed * -context.playerDirection, 0);
            context.rb.velocity = velocity;
        }
    }
}