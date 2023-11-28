using EditorSystems.Logger;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public abstract class ShootingEnemy : MonoBehaviour
    {
        [SerializeField]protected ProjectileType projectileType;

        protected Vector2 shootingDirection;
        protected Vector2 shootingPosition;
        protected float projectileSpeed;

        protected float attackSpeed;
        protected float timeElapsed;

        protected bool stopBehaviour = false;

        private void Start()
        {
            SetUpEnemy();
            CheckSetUp();
            OnStart();
        }

        protected abstract void OnStart();

        protected void Update()
        {
            if (stopBehaviour) return;

            if(timeElapsed >= attackSpeed)
            {
                //Not sure if necessary
                StopCoroutine(Attack());
                StartCoroutine(Attack());
                timeElapsed = 0;
            }
            timeElapsed += Time.deltaTime;
        }

        protected abstract void SetUpEnemy();
        private void CheckSetUp()
        {
            if(shootingDirection == Vector2.zero)
            {
                EditorLogger.LogErrror(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Has a shooting direction of (0,0)");
                Destroy(gameObject);
            }
            else if(projectileSpeed <= 0)
            {
                EditorLogger.LogErrror(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Projectile speed has to be bigger than 0");
                Destroy(gameObject);
            }
            else if (attackSpeed <= 0)
            {
                EditorLogger.LogErrror(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Attack speed has to be bigger than 0");
                Destroy(gameObject);
            }
            else if(shootingPosition == Vector2.zero)
            {
                EditorLogger.LogErrror(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Shooting position is (0,0)");
                Destroy(gameObject);
            }
        }

        protected abstract IEnumerator Attack();

        protected IEnumerator Shoot(float beforeShotAnimationSyncTime = 0, float afterShotAnimationSyncTime = 0)
        {
            yield return new WaitForSeconds(beforeShotAnimationSyncTime);
            //We need to check if the the enemy is killed or anything mid animation
            if (stopBehaviour) yield break;

            //This is cursed code that needs to be fixed
            GameObject projectile = EnemyProjectilePool.I.GetProjectile(projectileType);
            projectile.transform.position = shootingPosition;
            
            yield return new WaitForSeconds(afterShotAnimationSyncTime);
        }

    }
}