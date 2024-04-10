using UnityEngine;
using DevSystems.StateMachine.SpecificStates;

namespace Enemies.Plant
{
    public class PlantAttackState : ShootingState<PlantStateMachine>
    {
        private readonly Vector2 _shootingOffset;

        public PlantAttackState(PlantStateMachine context, float attackSpeed, float projectileSpeed) : base(context)
        {
            projectileType = ShootingEnemyLogic.ProjectileType.Bean;

            this.attackSpeed = attackSpeed;
            this.projectileSpeed = projectileSpeed;

            //TODO: set up a proper offset
            _shootingOffset = Vector2.zero;
        }

        public override void OnEnter()
        {
            HandleSpriteDirection();
            SetUpShot();
        }

        public override void OnUpdate()
        {
            HandleShot();
            CheckSwitchState();
        }

        protected override void CheckSwitchState()
        {
            if (context.isPlayerInSafeZone && context.canRun)
            {
                SwitchState(context.runState);
                return;
            }

            if (!context.isPlayerInAttackRange)
            {
                SwitchState(context.idleState);
            }
        }

        protected override void OnExit()
        {
            elapsedTime = 0;
            context.spriteRenderer.flipX = false;
        }

        private void HandleSpriteDirection()
        {
            if(context.playerDirection == context.transform.localScale.x)
            {
                context.spriteRenderer.flipX = true;
            }
        }

        private void SetUpShot()
        {
            shootingDirection = new Vector2(context.playerDirection, 0);
            shootingPosition = context.transform.position + (Vector3)_shootingOffset;
        }

        private void HandleShot()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > attackSpeed)
            {
                Shoot();
                elapsedTime = 0;
            }
        }

    }
}