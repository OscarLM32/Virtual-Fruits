using EditorSystems.Logger;
using System.Collections;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Enemies.ShootingEnemyLogic
{
    public abstract class ShootingEnemy : MonoBehaviour
    {
        [SerializeField] protected ProjectileType projectileType;

        [SerializeField] protected Vector2 shootingDirection;
        protected Vector2 shootingPosition;
        [SerializeField] protected float projectileSpeed;

        //TODO: this current attack speed is not the the real attack speed since it does not have into
        //account the time animating the attack
        [SerializeField] protected float attackSpeed;
        protected float timeElapsed;

        protected bool stopShooting = false;

        protected void Start()
        {
            OnStart();
            CheckSetUp();
        }

        protected abstract void OnStart();

        protected void Update()
        {
            if (stopShooting) return;

            if (timeElapsed >= attackSpeed)
            {
                //Not sure if necessary
                StopCoroutine(Attack());
                StartCoroutine(Attack());
                timeElapsed = 0;
            }
            timeElapsed += Time.deltaTime;
        }


        private void CheckSetUp()
        {
            if (shootingDirection == Vector2.zero)
            {
                EditorLogger.LogError(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Has a shooting direction of (0,0)");
                Destroy(gameObject);
            }
            else if (projectileSpeed <= 0)
            {
                EditorLogger.LogError(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Projectile speed has to be bigger than 0");
                Destroy(gameObject);
            }
            else if (attackSpeed <= 0)
            {
                EditorLogger.LogError(LoggingSystem.SHOOTING_ENEMY, $"{gameObject.name}: Attack speed has to be bigger than 0");
                Destroy(gameObject);
            }
        }

        protected abstract IEnumerator Attack();

        protected IEnumerator Shoot(float beforeShotAnimationSyncTime = 0, float afterShotAnimationSyncTime = 0)
        {
            yield return new WaitForSeconds(beforeShotAnimationSyncTime);
            //We need to check if the the enemy is killed or anything mid animation
            if (stopShooting) yield break;
            if (EnemyProjectilePool.I == null)
            {
                Debug.LogWarning("There is no projectile pool in the scene");
                yield break;
            }

            GameObject obj = EnemyProjectilePool.I.GetProjectile(projectileType);
            EnemyProjectile projectile = obj.GetComponent<EnemyProjectile>();

            projectile.SetUpProjectile(shootingPosition, shootingDirection, projectileSpeed);
            obj.SetActive(true);
            //projectile.transform.position = shootingPosition;

            yield return new WaitForSeconds(afterShotAnimationSyncTime);
        }

    }
}